using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterMonster : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject monster;
    [SerializeField] private ObstacleDrop obstacle;
    [SerializeField] private GameStateMachine gsm;
    [SerializeField] private PlayerStateMachine psm;
    [SerializeField] private Canvas fadeCanvas;
    [SerializeField] private AudioSource musicManager;
    [SerializeField] private AudioClip audioToPlay;

    private PlayerMovementV2 playerMov;
    private Animator playerAnim;
    private Animator monsterAnim;
    private Animator fadeAnim;
    private Animator musicAnim;
    private AudioSource cutsceneMusic;
    private Animator cutsceneMusicAnim;

    private void Start()
    {
        playerMov = player.GetComponent<PlayerMovementV2>();
        playerAnim = player.GetComponentInChildren<Animator>();
        monsterAnim = monster.GetComponentInChildren<Animator>();
        fadeAnim = fadeCanvas.GetComponentInChildren<Animator>();
        musicAnim = musicManager.GetComponent<Animator>();
        cutsceneMusic = GetComponentInChildren<AudioSource>();
        cutsceneMusicAnim = cutsceneMusic.GetComponent<Animator>();
    }

    public void StartCutscene()
    {
        // Setup
        gsm.currentAction = GameStateMachine.Action.CUTSCENE;
        psm.currentState = PlayerStateMachine.PlayerState.END;
        psm.EnableSelecting(false);
        playerMov.CancelMapTile();
        player.transform.position = new Vector3(-23.5f, 3.5f, 0f);
        monster.SetActive(true);
        monster.transform.position = new Vector3(-15.5f, 8.5f, 0f);

        StartCoroutine(HandleMovement());
    }

    private IEnumerator HandleMovement()
    {
        // Stop background music
        musicAnim.Play("FadeOut");
        cutsceneMusic.Play();
        cutsceneMusicAnim.Play("FadeIn");

        // Move player
        PlayerMovement.PlayAnimation(playerAnim, PlayerMovement.Direction.UP);
        yield return StartCoroutine(SmoothMovement(player.transform, new Vector3(-23.5f, 8.5f, 0)));
        playerAnim.Play("Idle");

        // Move monster
        MonsterMovement.PlayAnimation(monsterAnim, MonsterMovement.Direction.LEFT);
        yield return StartCoroutine(SmoothMovement(monster.transform, new Vector3(-20.5f, 8.5f, 0)));
        monsterAnim.Play("Idle");

        DialogManager.Instance.AddMessage(new HighMsg("Beware of the monster!"));
        yield return new WaitForSeconds(1.2f);

        // Drop Obstacle
        obstacle.Drop();
        yield return new WaitForSeconds(1.0f);
        DialogManager.Instance.AddMessage(new HighMsg("Now is your chance to escape!"));

        yield return new WaitForSeconds(1.6f);

        // Move the player to the left and start the fadeout
        fadeAnim.Play("FadeOut");
        cutsceneMusicAnim.Play("FadeOut");
        PlayerMovement.PlayAnimation(playerAnim, PlayerMovement.Direction.LEFT);
        yield return StartCoroutine(SmoothMovement(player.transform, new Vector3(-28.5f, 8.5f, 0)));
    }

    private IEnumerator SmoothMovement(Transform tr, Vector3 end, float moveTime = 0.3f)
    {
        float sqrRemainingDistance = (tr.localPosition - end).sqrMagnitude;
        float inverseMoveTime = 1 / moveTime;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(tr.localPosition, end, inverseMoveTime * Time.deltaTime);
            tr.localPosition = newPosition;
            sqrRemainingDistance = (tr.localPosition - end).sqrMagnitude;

            yield return null;
        }
    }
}

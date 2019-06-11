using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndRoom : MonoBehaviour
{

    [SerializeField] private Canvas fadeCanvas;
    [SerializeField] private AudioSource musicManager;

    private Animator fadeAnim;
    private Animator musicAnim;
    private bool entered = false;


    private void Start()
    {
        fadeAnim = fadeCanvas.GetComponentInChildren<Animator>();
        musicAnim = musicManager.GetComponent<Animator>();
    }

    private IEnumerator StartCutscene()
    {
            fadeAnim.Play("FadeOut");
            musicAnim.Play("FadeOut");

            yield return new WaitForSeconds(1.5f);

            SceneManager.LoadScene("EndScene");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = other.gameObject;

        if (go.tag == "Player" && !entered)
        {
            entered = true;
            StartCoroutine(StartCutscene());
        }
    }
}

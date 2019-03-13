using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerStateMachine))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Tilemap ground;
    [SerializeField] private float moveTime = 0.3f;

    [SerializeField] private GameObject dirArrow;

    private enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    /* Components */
    private PlayerStateMachine psm;

    /* Inputs */
    private float hInput = 0f;
    private float vInput = 0f;

    /* Attributes */
    private bool canMove = false;
    private bool isMoving = false;
    private Direction currentDir = Direction.UP;
    private Vector2 currentDirVec = new Vector2();

    private void Awake()
    {
        psm = GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (isMoving || !canMove) return;

        if (hInput != 0 || vInput != 0)
        {
            Vector2 newDir = new Vector2(hInput, vInput);
            GetNewDirection(newDir);
            canMove = false;
            //Move(newDir);
        }
    }

    public void MakePlayerMove()
    {
        Move(currentDirVec);
    }

    private void Move(Vector2 dir)
    {
        Vector2 pos = transform.position;
        Vector2 targetPos = pos + dir;
        StartCoroutine(SmoothMovement(targetPos));
    }

    private void GetNewDirection(Vector2 dir)
    {
        Debug.Log("Getting new Direction " + dir);

        if (dir == Vector2.up)
        {
            currentDir = Direction.UP;
            dirArrow.transform.eulerAngles = new Vector3(0, 0, 90f);
        }
        else if (dir == Vector2.down)
        {
            currentDir = Direction.DOWN;
            dirArrow.transform.eulerAngles = new Vector3(0, 0, -90f);
        }
        else if (dir == Vector2.right)
        {
            currentDir = Direction.RIGHT;
            dirArrow.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (dir == Vector2.left)
        {
            currentDir = Direction.LEFT;
            dirArrow.transform.eulerAngles = new Vector3(0, 0, 180f);
        }

        currentDirVec = dir;

        StartCoroutine(DebugWaiter(0.5f));
    }

    public IEnumerator DebugWaiter(float time)
    {
        yield return new WaitForSeconds(time);

        psm.FinishedSelecting();
    }

    private IEnumerator SmoothMovement(Vector3 end)
    {
        isMoving = true;

        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        float inverseMoveTime = 1 / moveTime;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }

        isMoving = false;
        psm.EndTurn();
    }

    private TileBase GetTile(Tilemap tmap, Vector2 pos)
    {
        return tmap.GetTile(GetTilePos(tmap, pos));
    }

    private Vector3Int GetTilePos(Tilemap tmap, Vector2 pos)
    {
        return tmap.WorldToCell(pos);
    }

    public void EnablePlayerMovement(bool enable)
    {
        canMove = enable;
    }
}
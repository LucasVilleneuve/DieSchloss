using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerStateMachine))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Tilemap ground;
    [SerializeField] private Tilemap collideable;
    [SerializeField] private GameObject interactiveObstaclesTilemap;
    [SerializeField] private GameObject dirArrow;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;

    [SerializeField] private float moveTime = 0.3f;

    private enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    /* Components */
    private PlayerStateMachine psm;
    private List<InteractiveObstacle> interactiveObstacles = new List<InteractiveObstacle>();
    private Animator anim;

    /* Inputs */
    private float hInput = 0f;
    private float vInput = 0f;

    /* Attributes */
    private bool canMove = false;
    private bool isMoving = false;
    private bool dirSelected = false;
    private bool isCurrentDirValid = false;
    private Direction currentDir = Direction.RIGHT;
    private Vector2 currentDirVec = new Vector2(1, 0);
    private bool confirmButtonPressed = false;
    public bool IsHiding = false;

    private Image dirArrowImg;

    private void Awake()
    {
        psm = GetComponent<PlayerStateMachine>();
        dirArrowImg = dirArrow.GetComponentInChildren<Image>();
        anim = GetComponentInChildren<Animator>();

        foreach (Transform child in interactiveObstaclesTilemap.transform)
        {
            Debug.Log("Adding a child " + child.gameObject);
            interactiveObstacles.Add(child.gameObject.GetComponent<InteractiveObstacle>());
        }
    }

    private void Update()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");
        confirmButtonPressed = Input.GetButton("Confirm");
    }

    private void FixedUpdate()
    {
        if (isMoving || !canMove) return;

        if (hInput != 0 || vInput != 0)
        {
            GetNewDirection(new Vector2(hInput, vInput));
        }

        if (isCurrentDirValid && confirmButtonPressed)
        {
            FinishedSelecting();
        }

        CheckIfCurrentDirIsValid();
    }

    public void MakePlayerMove()
    {
        StartCoroutine(Move(currentDirVec));
    }

    private IEnumerator Move(Vector2 dir)
    {
        Vector2 pos = transform.position;
        Vector2 targetPos = pos + dir;

        if (CanMoveToTile(targetPos))
        {
            //Debug.Log("Before moving");
            PlayAnimation(currentDir);
            yield return StartCoroutine(SmoothMovement(targetPos));
            if (IsHiding)
            {
                IsHiding = false;
                GetComponentsInChildren<SpriteRenderer>()[0].enabled = true;
            }
            StopAnimation();
            //Debug.Log("After moving");
        }
        else
        {
            // TODO Change direction and move there
        }

        psm.ClearCurrentAction();
        psm.EndTurn();
    }

    private void GetNewDirection(Vector2 dir)
    {
        float rotationAngle = 0f;

        if (dir.x != 0) dir.y = 0;

        if (dir == Vector2.up)
        {
            currentDir = Direction.UP;
            rotationAngle = 90f;
        }
        else if (dir == Vector2.down)
        {
            currentDir = Direction.DOWN;
            rotationAngle = -90f;
        }
        else if (dir == Vector2.right)
        {
            currentDir = Direction.RIGHT;
            rotationAngle = 0f;
        }
        else if (dir == Vector2.left)
        {
            currentDir = Direction.LEFT;
            rotationAngle = 180f;
        }

        dirArrow.transform.eulerAngles = new Vector3(0, 0, rotationAngle);
        currentDirVec = dir;
        CheckIfCurrentDirIsValid();
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

        //Debug.Log("Finished moving");
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
        CheckIfCurrentDirIsValid();
    }

    private void FinishedSelecting()
    {
        psm.FinishedSelecting();
    }

    private bool CanMoveToTile(Vector2 pos)
    {
        TileBase groundTile = GetTile(ground, pos);
        if (groundTile == null) return false;

        TileBase collideableTile = GetTile(collideable, pos);
        if (collideableTile != null) return false;

        InteractiveObstacle obstacleTile = GetInteractiveObstacle(interactiveObstacles, pos);
        if (obstacleTile != null && obstacleTile.IsBlocking()) return false;

        if (obstacleTile != null)
        {
            Debug.Log("ObstacleTile is blocking");
        }


        return true;
    }

    private void ChangeSpriteDirection(bool canMoveToDir)
    {
        dirArrowImg.sprite = (canMoveToDir) ? arrowSprite : crossSprite;
    }

    private void CheckIfCurrentDirIsValid()
    {
        Vector2 pos = transform.position;
        Vector2 targetPos = pos + currentDirVec;

        isCurrentDirValid = CanMoveToTile(targetPos);
        ChangeSpriteDirection(isCurrentDirValid);
    }

    public void ForceSelecting()
    {
        FinishedSelecting();
    }

    private InteractiveObstacle GetInteractiveObstacle(List<InteractiveObstacle> obstacles, Vector2 pos)
    {
        Vector2 obstaclePos;
        foreach (InteractiveObstacle obstacle in obstacles)
        {
            obstaclePos = obstacle.transform.position;
            if (IsWithinPos(pos.x, obstaclePos.x, obstaclePos.x + obstacle.width) &&
                IsWithinPos(pos.y, obstaclePos.y, obstaclePos.y + obstacle.height))
            {
                return obstacle;
            }
        }
        return null;
    }

    private bool IsWithinPos(float value, float floor, float top)
    {
        return (value >= floor && value < top);
    }

    private void PlayAnimation(Direction dir)
    {
        switch (dir)
        {
            case Direction.RIGHT:
                anim.Play("Walking Right");
            break;
            case Direction.LEFT:
                anim.Play("Walking Left");
            break;
            case Direction.UP:
                anim.Play("Walking Up");
            break;
            case Direction.DOWN:
                anim.Play("Walking Down");
            break;
        }
    }

    private void StopAnimation()
    {
        anim.Play("Idle");
    }
}
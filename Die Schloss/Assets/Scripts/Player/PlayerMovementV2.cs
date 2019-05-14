using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerStateMachine))]
public class PlayerMovementV2 : MonoBehaviour
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

    /* Zone of actions */

    public int range = 6;
    public int range2 = 9;
    public int range3 = 11;
    public float timeBetweenMove = 1f;
    public bool moveDisplay = false;
    private float nextMove;
    /* Components */
    private PlayerStateMachine psm;
    private List<InteractiveObstacle> interactiveObstacles = new List<InteractiveObstacle>();
    private Animator anim;

    /* Inputs */
    private float hInput = 0.0f;
    private float vInput = 0.0f;
    private int xChoice = 0;
    private int yChoice = 0;
    /* Attributes */
    private bool canMove = false;
    private bool isMoving = false;
    private bool dirSelected = false;
    private bool isCurrentDirValid = false;
    private Direction currentDir = Direction.RIGHT;
    private Vector2 currentDirVec = new Vector2(1, 0);
    private bool confirmButtonPressed = false;
    public bool IsHiding = false;
    public MapTile mapTile;
    private bool isInterract;

    private void Awake()
    {
        psm = GetComponent<PlayerStateMachine>();
        anim = GetComponentInChildren<Animator>();
        nextMove = Time.time;
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
        isInterract = Input.GetButton("Interact");
    }
    
    private bool UpDownTile(int reverse)
    {
        bool hasCreated = mapTile.CreateTileMouvement(0, 0);
        Vector2 pos = transform.position;
        for (int x = 0; x < range3; x++)
        {
            int horinzon = range3 - x;
            for (int y = 0; y < horinzon; y++)
            {
                if (CanMoveToTile(pos + new Vector2(x * reverse,    y)))
                    if (mapTile.CreateTileMouvement(x * reverse, y))
                        hasCreated = true;
            }
            for (int y = 0; y > -horinzon; y--)
            {
                if (CanMoveToTile(pos + new Vector2(x * reverse, y)))
                    if (mapTile.CreateTileMouvement(x * reverse, y))
                        hasCreated = true;
            }
        }
        return hasCreated;
    }

    private bool CreateMapMovement()
    {
        bool hasCreated = true;
        bool somethingWasCreated = false;
        while (hasCreated)
        {
            if ((hasCreated = UpDownTile(1)))
                somethingWasCreated = true;
            if (UpDownTile(-1))
            {
                hasCreated = true;
                somethingWasCreated = true;
            }
        }
        mapTile.SmoothMap();
        return somethingWasCreated;
    }

    private void SwitchFocusTile(bool xOrY, int minus)
    {
        mapTile.ChangeColorTile(new Vector2(xChoice, yChoice));
        if (xOrY)
            xChoice += minus;
        else
            yChoice += minus;
        mapTile.ChangeColorTile(new Vector2(xChoice, yChoice));
    }

    private void GetNewDirection(Vector2 dir)
    {
        float rotationAngle = 0f;
        
        if (dir.x != 0) dir.y = 0;
        if (dir == Vector2.up)
        {
            currentDir = Direction.UP;
            rotationAngle = 90f;
            if (mapTile.isTileExist(new Vector2(xChoice, yChoice + 1)) != null)
                    SwitchFocusTile(false, 1);
        }
        else if (dir == Vector2.down)
        {
            currentDir = Direction.DOWN;
            rotationAngle = -90f;
            if (mapTile.isTileExist(new Vector2(xChoice, yChoice - 1)) != null)
                SwitchFocusTile(false, -1);
        }
        else if (dir == Vector2.right)
        {
            currentDir = Direction.RIGHT;
            rotationAngle = 0f;
            if (mapTile.isTileExist(new Vector2(xChoice + 1, yChoice)) != null)
                SwitchFocusTile(true, 1);
        }
        else if (dir == Vector2.left)
        {
            currentDir = Direction.LEFT;
            rotationAngle = 180f;
            if (mapTile.isTileExist(new Vector2(xChoice - 1, yChoice)) != null)
                SwitchFocusTile(true, -1);
        }

        dirArrow.transform.eulerAngles = new Vector3(0, 0, rotationAngle);
        currentDirVec = dir;
        nextMove = Time.time + timeBetweenMove;
    }

    private void FixedUpdate()
    {
        if (isMoving || !canMove) return;
        if (!moveDisplay)
        {
            mapTile.SetRangeMap(range, range2, range3);
            xChoice = 0;
            yChoice = 0;
            CreateMapMovement();
            moveDisplay = true;
        }

        if (isInterract)
            CreateMapMovement();
        if (hInput != 0 || vInput != 0)
        {
            if (Time.time > nextMove)
                GetNewDirection(new Vector2(hInput, vInput));
        }
        if (confirmButtonPressed)
        {
            FinishedSelecting();
        }
        
    }

    public void MakePlayerMove()
    {
        StartCoroutine(Move(mapTile.ListPosition(new Vector2(xChoice, yChoice))));
    }

    private bool isReachingDestination(Vector2 dir)
    {

        return true;
    }

    private IEnumerator Move(Stack<Vector2> directions)
    {
        Vector2 pos = transform.position;

        mapTile.DeleteTiles();
        while (directions.Count != 0)
        {
            Vector2 dir = directions.Pop();
            PlayAnimation(currentDir);
            yield return StartCoroutine(SmoothMovement(dir));
            if (IsHiding)
            {
                IsHiding = false;
                GetComponentsInChildren<SpriteRenderer>()[0].enabled = true;
            }
            StopAnimation();
        }
        psm.ClearCurrentAction();
        psm.EndTurn();
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

    private void FinishedSelecting()
    {
        psm.FinishedSelecting();
        moveDisplay = false;
    }

    private bool CanMoveToTile(Vector2 pos)
    {
        TileBase groundTile = GetTile(ground, pos);
        if (groundTile == null) return false;

        TileBase collideableTile = GetTile(collideable, pos);
        if (collideableTile != null) return false;

        InteractiveObstacle obstacleTile = GetInteractiveObstacle(interactiveObstacles, pos);
        if (obstacleTile != null && obstacleTile.IsBlocking()) return false;

        return true;
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

    public void CancelMapTile()
    {
        mapTile.DeleteTiles();
    }
}
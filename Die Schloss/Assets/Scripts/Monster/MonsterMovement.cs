﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterMovement : MonoBehaviour
{

    [SerializeField] private Tilemap ground;
    [SerializeField] private Tilemap collideable;

    [SerializeField] private float moveTime = 0.3f;

    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NOWHERE,
    }

    private MonsterStateMachine msm;
    private MonsterBrain mb;

    private bool canMove = false;
    private bool isMoving = false;
    private bool dirSelected = false;
    private bool isCurrentDirValid = false;
    private Direction currentDir = Direction.RIGHT;
    private Vector2 currentDirVec = new Vector2(1, 0);
    // Start is called before the first frame update
    private void Awake()
    {
        msm = GetComponent<MonsterStateMachine>();
        mb = GetComponent<MonsterBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get Direction
    }

    public void MakeMonsterMove()
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
            yield return StartCoroutine(SmoothMovement(targetPos));
            //Debug.Log("After moving");
        }
        else
        {
            // TODO Change direction and move there
        }

        msm.ClearCurrentAction();
        msm.EndTurn();
    }

    private bool CanMoveToTile(Vector2 pos)
    {
        TileBase groundTile = GetTile(ground, pos);
        TileBase collideableTile = GetTile(collideable, pos);

        if (groundTile == null) return false;
        if (collideableTile != null) return false;
        return true;
    }

    private TileBase GetTile(Tilemap tmap, Vector2 pos)
    {
        return tmap.GetTile(GetTilePos(tmap, pos));
    }

    private Vector3Int GetTilePos(Tilemap tmap, Vector2 pos)
    {
        return tmap.WorldToCell(pos);
    }

    public void EnableMonsterMovement(bool enable)
    {
        canMove = enable;
        currentDir = mb.GetDirection();
        if (currentDir == Direction.UP)
        {
            currentDirVec = Vector2.up;
        }
        else if (currentDir == Direction.DOWN)
        {
            currentDirVec = Vector2.down;
        }
        else if (currentDir == Direction.RIGHT)
        {
            currentDirVec = Vector2.right;
        }
        else if (currentDir == Direction.LEFT)
        {
            currentDirVec = Vector2.left;
        }
        else
            currentDirVec = transform.position;
        CheckIfCurrentDirIsValid();
    }

    private void CheckIfCurrentDirIsValid()
    {
        Vector2 pos = transform.position;
        Vector2 targetPos = pos + currentDirVec;

        isCurrentDirValid = CanMoveToTile(targetPos);
        FinishedSelecting();
    }

    private void FinishedSelecting()
    {
        msm.FinishedSelecting();
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
}
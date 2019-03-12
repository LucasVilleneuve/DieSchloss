using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    private enum Direction
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    private Direction dir;

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        if (hInput >= 0.5)
            dir = Direction.RIGHT;
        else if (hInput <= -0.5)
            dir = Direction.LEFT;
        else if (vInput <= -0.5)
            dir = Direction.DOWN;
        else if (vInput >= 0.5)
            dir = Direction.UP;
    }

    public void ExecuteTurn()
    {
        // TODO Use item ?

        // Move
        Move();
    }

    private IEnumerator Move()
    {
        yield return new WaitUntil(() => dir != Direction.NONE);
        Debug.Log("Moving in direction of " + dir);
        dir = Direction.NONE;
    }
}
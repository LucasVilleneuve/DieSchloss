using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    TileObject[] surroundedTiles = new TileObject[4];
    public Vector2 position;
    private bool isActive = false;
    public Sprite active;
    public Sprite unActive;
    public int priorityTile = 1000;
    public enum Direction {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3}

    public void Start()
    {
    }

    public void SelectionTile()
    {
        if (isActive)
        {
            isActive = false;
            GetComponent<SpriteRenderer>().sprite = active;
        }
        else
        {
            isActive = true;
            GetComponent<SpriteRenderer>().sprite = unActive;
        }
    }

    public void DisplaySprite()
    {
        if (isActive)
            GetComponent<SpriteRenderer>().sprite = unActive;
        else
            GetComponent<SpriteRenderer>().sprite = active;
    }

    public bool PriorityChange()
    {
        int tmpPriority;
        int priorityEnd = priorityTile;
        if (priorityTile == 0)
            return (false);
        for (int i = 0;i < 4;i++)
            if (surroundedTiles[i] != null)
            {
                tmpPriority = surroundedTiles[i].priorityTile;
                if (tmpPriority < priorityEnd)
                    priorityEnd = tmpPriority;
            }
        if (priorityEnd < priorityTile - 1)
        {
            priorityTile = priorityEnd + 1;
            return (true);
        }
        return (false);
    }

    public void SetSprite(Sprite _active, Sprite _unActive)
    {
        active = _active;
        unActive = _unActive;
    }
    
    public void Destruction()
    {
        for (int i = 0; i < 4; i++)
        {
            if (surroundedTiles[i] != null)
                switch (i)
                {
                    case ((int)Direction.Up):
                        surroundedTiles[i].removeTileSurrounding(Direction.Down);
                        break;
                    case ((int)Direction.Down):
                        surroundedTiles[i].removeTileSurrounding(Direction.Up);
                        break;
                    case ((int)Direction.Right):
                        surroundedTiles[i].removeTileSurrounding(Direction.Left);
                        break;
                    case ((int)Direction.Left):
                        surroundedTiles[i].removeTileSurrounding(Direction.Right);
                        break;
                }
        }
    }

    public TileObject getTileCloser()
    {
        int tmpPriority;
        if (priorityTile == 0)
            return null;
        for (int i = 0; i < 4; i++)
            if (surroundedTiles[i] != null)
            {
                tmpPriority = surroundedTiles[i].priorityTile;
                if (priorityTile > tmpPriority)
                    return surroundedTiles[i];
            }
        return (null);
    }

    public Vector2 getTileTruePosition()
    {
        return this.transform.position;
    }

    public void addTileSurrounding(TileObject obj, Direction side)
    {
        surroundedTiles[(int)side] = obj;
    }

    public void removeTileSurrounding(Direction side)
    {
        surroundedTiles[(int)side] = null;
    }
}

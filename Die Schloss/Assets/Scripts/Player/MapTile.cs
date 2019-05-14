using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public Sprite[] tileSprite;
    public GameObject tileMovementPrefab;
    private List<TileObject> listTile = new List<TileObject>();
    private int range1;
    private int range2;
    private int range3;

    public void SetRangeMap(int range1T, int range2T, int range3T)
    {
        range1 = range1T;
        range2 = range2T;
        range3 = range3T;
    }

    public TileObject isTileExist(Vector2 pos)
    {
        for (int i = 0; i < listTile.Count; i++)
        {
            if (listTile[i] != null)
                if (listTile[i].position == pos)
                    return listTile[i];
        }
        return null;
    }

    public Stack<Vector2> ListPosition(Vector2 finalPosition)
    {
        Stack<Vector2> returnPath = new Stack<Vector2>();
        TileObject tmpTile;
        TileObject currentTile;

        currentTile = isTileExist(finalPosition);
        returnPath.Push(currentTile.getTileTruePosition());
        while ((tmpTile = currentTile.getTileCloser()) != null)
        {
            currentTile = tmpTile;
            returnPath.Push(currentTile.getTileTruePosition());
        }
        return returnPath;
    }

    public List<TileObject> ListSurrounded(int positionX, int positionY)
    {
        List<TileObject> listSurrounding = new List<TileObject>();

        listSurrounding.Add(isTileExist(new Vector2(positionX + 1, positionY)));
        listSurrounding.Add(isTileExist(new Vector2(positionX - 1, positionY)));
        listSurrounding.Add(isTileExist(new Vector2(positionX, positionY + 1)));
        listSurrounding.Add(isTileExist(new Vector2(positionX, positionY - 1)));
        return listSurrounding;
    }

    private void addSurroundings(TileObject currentTile)
    {
        List<TileObject> listSurrounding =
            ListSurrounded((int)currentTile.position.x, (int)currentTile.position.y);
        if (listSurrounding[0] != null)
        {
            currentTile.addTileSurrounding(listSurrounding[0], TileObject.Direction.Right);
            listSurrounding[0].addTileSurrounding(currentTile, TileObject.Direction.Left);
        }
        if (listSurrounding[1] != null)
        {
            currentTile.addTileSurrounding(listSurrounding[1], TileObject.Direction.Left);
            listSurrounding[1].addTileSurrounding(currentTile, TileObject.Direction.Right);
        }
        if (listSurrounding[2] != null)
        {
            currentTile.addTileSurrounding(listSurrounding[2], TileObject.Direction.Up);
            listSurrounding[2].addTileSurrounding(currentTile, TileObject.Direction.Down);
        }
        if (listSurrounding[3] != null)
        {
            currentTile.addTileSurrounding(listSurrounding[3], TileObject.Direction.Down);
            listSurrounding[3].addTileSurrounding(currentTile, TileObject.Direction.Up);
        }
    }

    public bool CreateTileMouvement(int positionX, int positionY)
    {
        if (isTileExist(new Vector2(positionX, positionY)) != null)
            return false;

        Vector3 position = new Vector3((float)positionX, (float)positionY, 0);
        GameObject obj = Instantiate(tileMovementPrefab) as GameObject;

        obj.transform.parent = this.transform;
        obj.transform.localPosition = position;

        TileObject currentTile = obj.GetComponent<TileObject>();

        currentTile.position = new Vector2(positionX, positionY);
        if (positionX != 0 || positionY != 0)
            currentTile.priorityTile = 1000;
        else
        {
            currentTile.priorityTile = 0;
            currentTile.SelectionTile();
        }
        addSurroundings(currentTile);
        listTile.Add(currentTile);
        return true;
    }

    private TileObject setFinaleTileAspect(TileObject currentTile)
    {
        if (currentTile == null)
            return null;
        if (currentTile.priorityTile < range1)
            currentTile.SetSprite(tileSprite[0], tileSprite[1]);
        else if (currentTile.priorityTile < range2)
            currentTile.SetSprite(tileSprite[2], tileSprite[3]);
        else if (currentTile.priorityTile < range3)
            currentTile.SetSprite(tileSprite[4], tileSprite[5]);
        else
        {
            currentTile.Destruction();
            Destroy(currentTile.gameObject);
            return null;
        }
        currentTile.DisplaySprite();
        return currentTile;
    }

    public void SmoothMap()
    {
        bool change = true;

        while (change)
        {
            change = false;
            for (int i = 0; i < listTile.Count; i++)
            {
                if (listTile[i] != null)
                    if (listTile[i].PriorityChange())
                        change = true;
            }
        }
        for (int i = 0; i < listTile.Count; i++)
            listTile[i] = setFinaleTileAspect(listTile[i]);
    }

    public void DeleteTiles()
    {
        for (int i = listTile.Count - 1; i >= 0; i--)
        {
            if (listTile[i] != null)
                Destroy(listTile[i].gameObject);
        }
        listTile.Clear();
    }
    
    public void ChangeColorTile(Vector2 position)
    {
        TileObject tileSearched;

        tileSearched = isTileExist(position);
        if (tileSearched == null)
            return;
        tileSearched.SelectionTile();
    }
}

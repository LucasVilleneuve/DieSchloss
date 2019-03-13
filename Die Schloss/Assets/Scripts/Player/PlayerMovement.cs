using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Tilemap ground;
    [SerializeField] private float moveTime = 0.3f;

    [SerializeField] private GameObject dirArrow;

    private bool isMoving = false;

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        if (isMoving) return;

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        hInput = Mathf.Round(hInput);
        vInput = Mathf.Round(vInput);

        if (hInput != 0 || vInput != 0)
        {
            Move(new Vector2(hInput, vInput));
        }
    }

    private void Move(Vector2 dir)
    {
        Vector2 pos = transform.position;
        Vector2 targetPos = pos + dir;
        if (dir == Vector2.up) dirArrow.transform.eulerAngles = new Vector3(0, 0, 90);
        else if (dir == Vector2.down) dirArrow.transform.eulerAngles = new Vector3(0, 0, -90);
        else if (dir == Vector2.right) dirArrow.transform.eulerAngles = new Vector3(0, 0, 0);
        else if (dir == Vector2.left) dirArrow.transform.eulerAngles = new Vector3(0, 0, 180);
        StartCoroutine(SmoothMovement(targetPos));
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
}
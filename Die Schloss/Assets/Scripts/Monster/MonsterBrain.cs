using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterBrain : MonoBehaviour
{

    [SerializeField] private Tilemap ground;
    [SerializeField] private Tilemap collideable;
    [SerializeField] private GameObject prey;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public MonsterMovement.Direction GetDirection()
    {
        List<Vector3> path = Pathfinding.AStar.FindPath(collideable, transform.position, prey.transform.position);
        if (path == null || path.Count <= 2)
            return MonsterMovement.Direction.NOWHERE;
        else if (path[1].x > transform.position.x)
        {
            return MonsterMovement.Direction.RIGHT;
        }
        else if (path[1].x < transform.position.x)
        {
                return MonsterMovement.Direction.LEFT;
        }
        else if (path[1].y > transform.position.y)
        {
            return MonsterMovement.Direction.UP;
        }
        else if (path[1].y < transform.position.y)
        {
            return MonsterMovement.Direction.DOWN;
        }
        return MonsterMovement.Direction.NOWHERE;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterBrain : MonoBehaviour
{

    [SerializeField] private Tilemap ground;
    [SerializeField] private Tilemap collideable;
    [SerializeField] private GameObject prey;
    private Animator anim;
    private Vector3 lastNoisePos;
    private bool isThereNoise = false;
    private int sight = 5;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IMadeNoise(Vector3 pos, int intensity)
    {
        List<Vector3> path = Pathfinding.AStar.FindPath(collideable, transform.position, pos);
        if (path == null) return;
        if (path.Count <= intensity)
        {
            isThereNoise = true;
            lastNoisePos = pos;
        }
    }

    public MonsterMovement.Direction GetDirection()
    {
        List<Vector3> path = Pathfinding.AStar.FindPath(collideable, transform.position, prey.transform.position);
        if (path == null)
            return MonsterMovement.Direction.NOWHERE;

        Debug.Log("PATHCOUNT = " + path.Count);

        if (path.Count > sight) // If player is NOT in sight
        {
            if (isThereNoise)
            {
                List<Vector3> noisePath = Pathfinding.AStar.FindPath(collideable, transform.position, lastNoisePos);
                if (noisePath.Count <= sight)
                {
                    isThereNoise = false;
                    return Wander();
                }
                Debug.Log("[Monster] Following noise");
                if (noisePath[1].x > transform.position.x)
                {
                    return MonsterMovement.Direction.RIGHT;
                }
                else if (noisePath[1].x < transform.position.x)
                {
                    return MonsterMovement.Direction.LEFT;
                }
                else if (noisePath[1].y > transform.position.y)
                {
                    return MonsterMovement.Direction.UP;
                }
                else if (noisePath[1].y < transform.position.y)
                {
                    return MonsterMovement.Direction.DOWN;
                }
                return MonsterMovement.Direction.NOWHERE;
            }
            else
            {
                return Wander();
            }
        }
        else // If player is in sight
        {
            Debug.Log("[Monster] Following Player");
            if (path == null || path.Count <= 2)
            {
                return MonsterMovement.Direction.NOWHERE;
            }
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
        }
        return MonsterMovement.Direction.NOWHERE;
    }

    private MonsterMovement.Direction Wander()
    {
        Debug.Log("[Monster] Wandering");
        return (MonsterMovement.Direction)Random.Range(0, 3);
    }

    public bool IsInRange()
    {
        List<Vector3> path = Pathfinding.AStar.FindPath(collideable, transform.position, prey.transform.position);
        if (path != null && path.Count <= 2)
            return true;
        return false;
    }

    public void ActDead( bool isDead)
    {

        anim.SetBool("isDead", isDead);
    }
}

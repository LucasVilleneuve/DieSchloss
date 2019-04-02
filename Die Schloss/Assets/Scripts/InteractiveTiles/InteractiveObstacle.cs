using UnityEngine;

public class InteractiveObstacle : MonoBehaviour
{
    public int height = 1;
    public int width = 1;

    private bool blocking = true;

    public bool IsBlocking()
    {
        return blocking;
    }

    public void SetBlocking(bool block)
    {
        blocking = block;
    }
}
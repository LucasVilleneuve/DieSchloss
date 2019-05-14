using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDrop : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private float moveTime;

    private bool isMoving = false;
    private AudioSource audio;

    private void Start()
    {
        audio = GetComponentInChildren<AudioSource>();
    }

    public void Drop()
    {
        this.gameObject.SetActive(true);
        this.transform.position = startPosition;

        StartCoroutine(SmoothMovement(endPosition));
    }

    private IEnumerator SmoothMovement(Vector3 end)
    {
        isMoving = true;

        float sqrRemainingDistance = (transform.localPosition - end).sqrMagnitude;
        float inverseMoveTime = 1 / moveTime;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.localPosition, end, inverseMoveTime * Time.deltaTime);
            transform.localPosition = newPosition;
            sqrRemainingDistance = (transform.localPosition - end).sqrMagnitude;

            yield return null;
        }

        isMoving = false;
        audio.Play();
    }

}

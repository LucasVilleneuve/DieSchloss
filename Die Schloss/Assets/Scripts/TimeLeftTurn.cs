using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLeftTurn : MonoBehaviour
{
    [SerializeField] private float turnTime = 5.0f;
    [SerializeField] private PlayerStateMachine psm;

    /* Attributes */
    public float timeLeft; // public for debug reasons
    private bool running = false;

    private void Start()
    {
        timeLeft = turnTime;
    }

    private void Update()
    {
        if (running)
        {
            timeLeft -= Time.deltaTime;
            SetSize(timeLeft / turnTime);

            if (timeLeft <= 0)
            {
                psm.NotifyTimeIsUp();
                StopTimer();
            }
        }
    }

    public void Reset()
    {
        timeLeft = turnTime;
        SetSize(1.0f);
        running = false;
    }

    private void SetSize(float normalizedSize)
    {
        if (normalizedSize < 0)
            normalizedSize = 0;
        transform.localScale = new Vector2(normalizedSize, 1);
    }

    public void StartTimer()
    {
        running = true;
    }

    public void StopTimer()
    {
        running = false;
    }
}
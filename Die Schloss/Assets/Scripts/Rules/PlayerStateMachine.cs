using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum PlayerState
    {
        WAIT,
        SELECTING,
        PERFORMACTION,
        END
    }

    // PlayerScript player;
    public PlayerState currentState = PlayerState.WAIT;

    private GameStateMachine gsm;

    private void Start()
    {
        Debug.Log("Player start");
        gsm = GameObject.FindGameObjectWithTag("GameStateMachine").GetComponent<GameStateMachine>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case (PlayerState.WAIT):
                break;

            case (PlayerState.SELECTING):
                SelectAction();
                break;

            case (PlayerState.PERFORMACTION):
                PerformAction();
                break;

            case (PlayerState.END):
                break;
        }
    }

    private void SelectAction()
    {
        HandleTurn action = new HandleTurn();
        HandleTurn endTurn = new HandleTurn();

        // Perform action
        action.type = "Player";
        endTurn.type = "PlayerEndTurn";
        Debug.Log("Selecting Action");

        gsm.CollectAction(action);
        gsm.CollectAction(action);
        gsm.CollectAction(endTurn);

        currentState = PlayerState.WAIT;

        //currentState = PlayerState.PERFORMACTION;
    }

    private void PerformAction()
    {
        // Do action
        Debug.Log("Performing action");

        // If final action
        //gsm.EndPlayerTurn();
        currentState = PlayerState.WAIT;
    }
}
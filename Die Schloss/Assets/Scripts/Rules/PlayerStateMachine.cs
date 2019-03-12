using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum TurnState
    {
        START,
        WAIT,
        SELECTING,
        ACTION,
        END
    }

    // PlayerScript player;
    public TurnState currentState = TurnState.START;

    private GameStateMachine gsm;

    private void Start()
    {
        Debug.Log("Player start");
        //currentState = TurnState.IDLE;
        gsm = GameObject.FindGameObjectWithTag("GameStateMachine").GetComponent<GameStateMachine>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case (TurnState.START):
                break;

            case (TurnState.WAIT):
                break;

            case (TurnState.SELECTING):
                SelectAction();
                break;

            case (TurnState.ACTION):
                PerformAction();
                break;

            case (TurnState.END):
                break;
        }
    }

    private void SelectAction()
    {
        HandleTurn action = new HandleTurn();

        // Perform action
        action.type = "Player";
        Debug.Log("Selecting Action");

        gsm.CollectAction(action);

        currentState = TurnState.ACTION;
    }

    private void PerformAction()
    {
        // Do action
        Debug.Log("Performing action");

        gsm.EndPlayerTurn();
    }
}
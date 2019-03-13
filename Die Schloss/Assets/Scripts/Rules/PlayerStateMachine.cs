using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private Canvas selectionCanvas;

    public enum PlayerState
    {
        WAIT,
        SELECTING,
        PERFORMACTION,
        END
    }

    /* Components */
    private PlayerMovement playerMov;
    private GameStateMachine gsm;

    public PlayerState currentState = PlayerState.WAIT;
    public HandleTurn currentAction = null;

    private void Start()
    {
        Debug.Log("Player start");
        gsm = GameObject.FindGameObjectWithTag("GameStateMachine").GetComponent<GameStateMachine>();
        playerMov = GetComponent<PlayerMovement>();
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
        Debug.Log("Start of selecting state");

        EnableSelecting(true);

        currentState = PlayerState.WAIT;
    }

    public void FinishedSelecting()
    {
        Debug.Log("Player finished selecting");

        EnableSelecting(false);

        HandleTurn action = new HandleTurn
        {
            type = "Player",
            action = "Move"
        };

        gsm.CollectAction(action);
    }

    private void PerformAction()
    {
        // Do action
        Debug.Log("Performing action");

        if (currentAction != null)
        {
            switch (currentAction.action)
            {
                case ("Move"):
                    playerMov.MakePlayerMove();
                    break;
            }
        }

        currentState = PlayerState.WAIT;
    }

    public void EnableSelecting(bool enable)
    {
        Debug.Log("Enabling canvas and movement : " + enable);

        selectionCanvas.enabled = enable;
        playerMov.EnablePlayerMovement(enable);
    }

    public void EndTurn()
    {
        HandleTurn endturn = new HandleTurn
        {
            type = "PlayerEndTurn"
        };

        gsm.CollectAction(endturn);
    }
}
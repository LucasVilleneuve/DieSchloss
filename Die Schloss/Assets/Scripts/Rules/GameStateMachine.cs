using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public enum Action
    {
        START,
        WAIT,
        PROCESSACTION,
        GAMETURN,
        PLAYERTURN,
        ENEMYTURN,
        END
    }

    public Action currentAction;
    public Queue<HandleTurn> actions = new Queue<HandleTurn>();
    private PlayerStateMachine player;

    private void Start()
    {
        //Debug.Log("GSM start");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
        currentAction = Action.START;
    }

    private void Update()
    {
        switch (currentAction)
        {
            case (Action.START):
                currentAction = Action.GAMETURN;
                break;

            case (Action.WAIT):
                if (actions.Count > 0)
                {
                    currentAction = Action.PROCESSACTION;
                }
                break;

            case (Action.PROCESSACTION):
                ProcessActions();
                break;

            case (Action.GAMETURN):
                // Do stuff
                currentAction = Action.PLAYERTURN;
                break;

            case (Action.PLAYERTURN):
                PlayerTurn();
                break;

            case (Action.ENEMYTURN):
                // Do stuff
                currentAction = Action.END;
                break;

            case (Action.END):
                currentAction = Action.START;
                break;
        }
    }

    public void CollectAction(HandleTurn action)
    {
        actions.Enqueue(action);
    }

    private void ProcessActions()
    {
        Debug.Log("Processing action");
        HandleTurn turn = actions.Dequeue();

        if (turn.type == "Player")
        {
            player.currentAction = turn;
            player.currentState = PlayerStateMachine.PlayerState.PERFORMACTION;
        }
        else if (turn.type == "PlayerEndTurn")
        {
            player.currentState = PlayerStateMachine.PlayerState.WAIT;
            EndPlayerTurn();
            return;
        }

        currentAction = Action.WAIT;
    }

    private void PlayerTurn()
    {
        //Debug.Log("It's player's turn");
        player.currentState = PlayerStateMachine.PlayerState.SELECTING;
        currentAction = Action.WAIT;
    }

    public void EndPlayerTurn()
    {
        currentAction = Action.ENEMYTURN;
        player.currentState = PlayerStateMachine.PlayerState.WAIT;
        Debug.Log("End of player turn");
    }
}
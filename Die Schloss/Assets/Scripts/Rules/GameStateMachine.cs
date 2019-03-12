using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public enum Action
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        START,
        GAMETURN,
        PLAYERTURN,
        ENEMYTURN,
        END
    }

    public Action currentAction;
    public List<HandleTurn> actions = new List<HandleTurn>();
    private PlayerStateMachine player;

    private void Start()
    {
        Debug.Log("GSM start");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
        currentAction = Action.START;
        //player.currentState = PlayerStateMachine.TurnState.SELECTING;
    }

    private void Update()
    {
        switch (currentAction)
        {
            case (Action.WAIT):
                //if (actions.Count > 0)
                //{
                //    currentAction = Action.TAKEACTION;
                //}
                //else
                //{
                //    Debug.Log("End of turn");
                //}
                break;

            case (Action.TAKEACTION):
                //ProcessActions();
                break;

            case (Action.PERFORMACTION):
                break;

            case (Action.START):
                currentAction = Action.GAMETURN;
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
        actions.Add(action);
    }

    private void ProcessActions()
    {
        HandleTurn turn = actions[0];
        actions.RemoveAt(0);

        if (turn.type == "Player")
        {
            player.currentState = PlayerStateMachine.TurnState.ACTION;
        }

        currentAction = Action.PERFORMACTION;
    }

    private void PlayerTurn()
    {
        Debug.Log("It's player's turn");
        player.currentState = PlayerStateMachine.TurnState.SELECTING;
        currentAction = Action.WAIT;
    }

    //public void MakeStateMachineWait()
    //{
    //    currentAction = Action.WAIT;
    //}

    public void EndPlayerTurn()
    {
        currentAction = Action.ENEMYTURN;
        player.currentState = PlayerStateMachine.TurnState.WAIT;
        Debug.Log("End of player turn");
    }
}
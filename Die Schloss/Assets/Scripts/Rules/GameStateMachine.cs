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
    private MonsterStateMachine monster;

    private void Start()
    {
        Debug.Log("GSM start");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
        monster = GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterStateMachine>();

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
                MonsterTurn();
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
        if (turn.type == "Monster")
        {
            monster.currentAction = turn;
            monster.currentState = MonsterStateMachine.MonsterState.PERFORMACTION;
        }
        else if (turn.type == "MonsterEndTurn")
        {
            monster.currentState = MonsterStateMachine.MonsterState.WAIT;
            EndMonsterTurn();
            return;
        }

        currentAction = Action.WAIT;
    }

    private void PlayerTurn()
    {
        Debug.Log("It's player's turn");
        player.currentState = PlayerStateMachine.PlayerState.SELECTING;
        currentAction = Action.WAIT;
    }

    public void EndPlayerTurn()
    {
        currentAction = Action.ENEMYTURN;
        player.currentState = PlayerStateMachine.PlayerState.WAIT;
        Debug.Log("End of player turn");
    }

    private void MonsterTurn()
    {
        Debug.Log("It's monster's turn");
        monster.currentState = MonsterStateMachine.MonsterState.SELECTING;
        currentAction = Action.WAIT;
    }

    public void EndMonsterTurn()
    {
        currentAction = Action.END;
        monster.currentState = MonsterStateMachine.MonsterState.WAIT;
        Debug.Log("End of player turn");
    }
}
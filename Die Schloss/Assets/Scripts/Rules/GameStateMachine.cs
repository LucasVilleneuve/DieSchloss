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
        GameObject monsterGo = GameObject.FindGameObjectWithTag("Monster");
        if (monsterGo)
            monster = monsterGo.GetComponent<MonsterStateMachine>();

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
        player.currentState = PlayerStateMachine.PlayerState.SELECTING;
        currentAction = Action.WAIT;
    }

    public void EndPlayerTurn()
    {
        currentAction = Action.ENEMYTURN;
        player.currentState = PlayerStateMachine.PlayerState.WAIT;
    }

    private void MonsterTurn()
    {
        if (monster)
        {
            monster.currentState = MonsterStateMachine.MonsterState.SELECTING;
            currentAction = Action.WAIT;
        }
        else
        {
            currentAction = Action.END;
        }
    }

    public void EndMonsterTurn()
    {
        currentAction = Action.END;
        monster.currentState = MonsterStateMachine.MonsterState.WAIT;
        Debug.Log("End of player turn");
    }
}
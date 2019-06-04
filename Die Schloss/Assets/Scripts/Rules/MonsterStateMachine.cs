using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : MonoBehaviour
{
    [SerializeField] private GameObject[] selectionCanvas;

    public enum MonsterState
    {
        WAIT,
        SELECTING,
        PERFORMACTION,
        END
    }

    private MonsterMovement monsterMov;
    private GameStateMachine gsm;
    private MonsterAttack ma;
    private MonsterBrain mb;
    public int current_turn = 0;
    private int last_turn = 0;
    private bool isDead = false;
    private int TurnDead = 0;

    public MonsterState currentState = MonsterState.WAIT;
    public HandleTurn currentAction = null;
    public bool isCurrentlySelecting = false;
   
    // Start is called before the first frame update
    void Start()
    {
        gsm = GameObject.FindGameObjectWithTag("GameStateMachine").GetComponent<GameStateMachine>();
        monsterMov = GetComponent<MonsterMovement>();
        mb = GetComponent<MonsterBrain>();
        ma = GetComponent<MonsterAttack>();
    }


    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (MonsterState.WAIT):
                break;
            case (MonsterState.SELECTING):
                SelectAction();
                break;
            case (MonsterState.PERFORMACTION):
                PerformAction();
                break;
            case (MonsterState.END):
                break;
        }
    }

    private void SelectAction()
    {
        if (!isDead)
        {
            current_turn += 1;
            currentState = MonsterState.WAIT;
            EnableSelecting(true);
        }
        else
        {
            currentState = MonsterState.WAIT;
            TurnDead--;
            if (TurnDead <= 0)
            {
                isDead = false;
                mb.ActDead(isDead);
            }
            EndTurn();
        }
    }

    public void EnableSelecting(bool enable)
    {
        isCurrentlySelecting = enable;
        if (enable)
            monsterMov.EnableMonsterMovement(enable);
    }

    public void FinishedSelecting()
    {
        //Debug.Log("Player finished selecting");

        EnableSelecting(false);

        HandleTurn action = new HandleTurn
        {
            type = "Monster",
            action = "Move"
        };

        gsm.CollectAction(action);
    }

    private void PerformAction()
    {
        if (current_turn == last_turn)
            return;
        else
        {
            last_turn += 1;
        }

        if (currentAction != null)
        {
            switch (currentAction.action)
            {
                case ("Move"):
                    ClearCurrentAction();
                    currentState = MonsterState.WAIT;
                    monsterMov.MakeMonsterMove();
                    break;
            }
        }

        currentState = MonsterState.WAIT;
    }

    public void ClearCurrentAction()
    {
        currentAction = null;
    }

    public void EndTurn()
    {
        HandleTurn endturn = new HandleTurn
        {
            type = "MonsterEndTurn"
        };

        gsm.CollectAction(endturn);
    }

    public void CheckForAttack()
    {
        if (mb.IsInRange())
        {
            StartCoroutine(ma.Atack());
            isDead = true;
            TurnDead = 4;
        }
        else
            EndTurn();
    }
}

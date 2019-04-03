using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private GameObject[] selectionCanvas;
    [SerializeField] private TimeLeftTurn timeLeftTurn;

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

    /* Public attributes */
    public PlayerState currentState = PlayerState.WAIT;
    public HandleTurn currentAction = null;
    public bool isCurrentlySelecting = false;

    private void Start()
    {
        //Debug.Log("Player start");
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
        //Debug.Log("Start of selecting state");

        EnableSelecting(true);
        timeLeftTurn.Reset();
        timeLeftTurn.StartTimer();
        currentState = PlayerState.WAIT;
    }

    public void FinishedSelecting()
    {
        //Debug.Log("Player finished selecting");

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
        //Debug.Log("Performing action : " + currentAction.type + " - " + currentAction.action);

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
        //Debug.Log("Enabling canvas and movement : " + enable);

        isCurrentlySelecting = enable;
        playerMov.EnablePlayerMovement(enable);
        foreach (GameObject canvas in selectionCanvas)
        {
            canvas.SetActive(enable);
        }
        if (enable)
            timeLeftTurn.StartTimer();
        else
            timeLeftTurn.StopTimer();
        timeLeftTurn.Reset();
    }

    public void EndTurn()
    {
        HandleTurn endturn = new HandleTurn
        {
            type = "PlayerEndTurn"
        };

        gsm.CollectAction(endturn);
    }

    public void ClearCurrentAction()
    {
        currentAction = null;
    }

    public bool IsPlayerInSelectingState()
    {
        return isCurrentlySelecting;
    }

    public void NotifyTimeIsUp()
    {
        playerMov.ForceSelecting();
    }
}
using UnityEngine;

public class Door : MonoBehaviour
{
    public int idKeyItemAssociated = -1;
    public bool isLocked = true;

    private PlayerInventory playerInv;
    private PlayerStateMachine psm;
    private Canvas canvasInteraction;
    private Animator anim;

    private bool playerClose = false;
    private bool openInputPressed = false;

    void Awake()
    {
        GameObject playerGo = GameObject.FindGameObjectWithTag("Player");
        playerInv = playerGo.GetComponent<PlayerInventory>();
        psm = playerGo.GetComponent<PlayerStateMachine>();
        canvasInteraction = GetComponentInChildren<Canvas>();
        anim = GetComponent<Animator>();
        anim.SetBool("locked", isLocked);
    }

    private void Update()
    {
        openInputPressed = Input.GetButton("Interact");

        if (playerClose && psm.IsPlayerInSelectingState())
        {
            if (isLocked)
            {
                EnableCanvas(true);
                if (openInputPressed)
                {
                    Open();
                }
            }
        }
        else
        {
            EnableCanvas(false);
        }
    }


    /// <summary>
    /// If the player has the key item associated, opens the door.
    /// Removes the key item from the player inventory if the door is opened.
    /// </summary>
    public void Open()
    {
        if (isLocked == false) 
        {
            Debug.Log("Door already opened");
            return;
        }

        UsableObject obj = playerInv.Get(idKeyItemAssociated);

        if (obj is null || obj.id == -1)
        {
            Debug.Log("The player does not have the key. Cannot open the door.");
            return;
        }

        Debug.Log("The door is now opened");

        EnableCanvas(false);

        playerInv.Remove(obj);

        isLocked = false;
        anim.SetBool("locked", false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject go = collider.gameObject;

        if (go.tag != "Player") return;

        Debug.Log("Player close");
        playerClose = true;        
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        GameObject go = collider.gameObject;

        if (go.tag != "Player") return;

        playerClose = false;
    }

    private void EnableCanvas(bool enable)
    {
        if (enable == canvasInteraction.enabled) return;
        canvasInteraction.enabled = enable;
    }
}

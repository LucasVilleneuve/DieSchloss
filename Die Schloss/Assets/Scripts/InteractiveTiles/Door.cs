using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : InteractiveObstacle
{
    public bool needKey = true;
    public int idKeyItemAssociated = -1;
    public bool isLocked = true;

    private PlayerInventory playerInv;
    private PlayerStateMachine psm;
    private Canvas canvasInteraction;
    private Animator anim;
    [SerializeField] private TileBase tile;


    private Tilemap collidable;

    private bool playerClose = false;
    private bool openInputPressed = false;

    private void Awake()
    {
        GameObject playerGo = GameObject.FindGameObjectWithTag("Player");
        playerInv = playerGo.GetComponent<PlayerInventory>();
        psm = playerGo.GetComponent<PlayerStateMachine>();
        canvasInteraction = GetComponentInChildren<Canvas>();
        anim = GetComponent<Animator>();
        collidable = GameObject.Find("Tilemap_Collideable").GetComponentInChildren<Tilemap>();
    }

    private void Start()
    {
        Lock(isLocked);
    }

    protected void Update()
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

        if (needKey)
        {
            UsableObject obj = TryToGetKey();

            if (obj is null)
                return;

            playerInv.Remove(obj.id);
        }

        Debug.Log("The door is now opened");

        EnableCanvas(false);

        Lock(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject go = collider.gameObject;

        if (go.tag != "Player") return;

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

    private void Lock(bool enable)
    {
        isLocked = enable;
        SetBlocking(enable);
        anim.SetBool("locked", enable);
        if (enable)
            collidable.SetTile(collidable.WorldToCell(transform.position), tile);
        else
            collidable.SetTile(collidable.WorldToCell(transform.position), null);
    }

    public UsableObject TryToGetKey()
    {
        UsableObject obj = playerInv.Get(idKeyItemAssociated);

        if (obj is null || obj.id == -1)
        {
            Debug.Log("The player does not have the key. Cannot open the door.");
            return null;
        }

        return obj;
    }

    public bool isPlayerClose()
    {
        return playerClose;
    }

    public bool isOpenInputPressed()
    {
        return openInputPressed;
    }

    public bool isDoorLocked()
    {
        return isLocked;
    }
}

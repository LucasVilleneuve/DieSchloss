using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UsableObject))]
public class PickUp : MonoBehaviour
{
    protected PlayerStateMachine psm;
    private PlayerInventory pInv;
    private MonsterBrain mBrain;
    private GameObject playerGo;
    private Canvas canvasInteraction;
    private UsableObject obj;

    protected bool pickUpInputPressed = false;
    protected bool playerClose = false;

    protected void Awake()
    {
        playerGo = GameObject.FindGameObjectWithTag("Player");
        psm = playerGo.GetComponent<PlayerStateMachine>();
        pInv = playerGo.GetComponent<PlayerInventory>();
        mBrain = GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterBrain>();
        canvasInteraction = GetComponentInChildren<Canvas>();
        obj = GetComponent<UsableObject>();
    }

    protected virtual void Update()
    {
        pickUpInputPressed = Input.GetButton("Interact");

        if (playerClose && psm.IsPlayerInSelectingState())
        {
            EnableCanvas(true);

            if (pickUpInputPressed)
            {
                Pick();
            }
        }
        else
        {
            EnableCanvas(false);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject go = collider.gameObject;

        if (go.tag != "Player") return;

        playerClose = true;
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        GameObject go = collider.gameObject;

        if (go.tag != "Player") return;

        playerClose = false;
    }

    virtual protected void Pick()
    {
        Debug.Log("Collecting " + obj.ToString());

        pInv.Add(obj);
        mBrain.IMadeNoise(gameObject.transform.position, 500);
        EnableCanvas(false);
        Destroy(this.gameObject);
    }

    protected void EnableCanvas(bool enable)
    {
        if (enable == canvasInteraction.enabled) return;
        canvasInteraction.enabled = enable;
    }
}
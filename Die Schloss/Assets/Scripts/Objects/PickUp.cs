using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private PlayerStateMachine psm;
    private GameObject playerGo;
    private Canvas canvasInteraction;

    private bool pickUpInputPressed = false;
    private bool playerClose = false;

    private void Awake()
    {
        playerGo = GameObject.FindGameObjectWithTag("Player");
        psm = playerGo.GetComponent<PlayerStateMachine>();
        canvasInteraction = GetComponentInChildren<Canvas>();
    }

    private void Update()
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject go = collider.gameObject;

        if (go.tag != "Player") return;

        playerClose = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        GameObject go = collider.gameObject;

        if (go.tag != "Player") return;

        playerClose = false;
    }

    private void Pick()
    {
        Debug.Log("Collected");
        // TODO Place in inventory
        EnableCanvas(false);
        Destroy(this.gameObject);
    }

    private void EnableCanvas(bool enable)
    {
        if (enable == canvasInteraction.enabled) return;
        canvasInteraction.enabled = enable;
    }
}
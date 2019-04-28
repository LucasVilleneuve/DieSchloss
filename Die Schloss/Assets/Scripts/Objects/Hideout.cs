using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideout : PickUp
{

    private GameObject Player;
    private SpriteRenderer sprite;
    private bool isHiding = false;

    protected override void Update()
    {
        pickUpInputPressed = Input.GetButton("Interact");
        if (playerClose && psm.IsPlayerInSelectingState() && !isHiding)
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

    protected override void Pick()
    {    
        Player.transform.position = transform.position;
        isHiding = true;
        sprite.enabled = false;
        Player.GetComponent<PlayerMovement>().IsHiding = true;
        EnableCanvas(false);
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Player = collider.gameObject;
        if (Player.tag != "Player")
        {
            Player = null;
            return;
        }
        sprite = Player.GetComponentsInChildren<SpriteRenderer>()[0];
        playerClose = true;
    }

    protected override void OnTriggerExit2D(Collider2D collider)
    {
       if (!Player || Player.tag != "Player") return;
        sprite.enabled = true;
        isHiding = false;
        playerClose = false;
    }
}

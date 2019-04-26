using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRoom : MonoBehaviour
{
    private bool entered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = other.gameObject;

        if (go.tag == "Player" && !entered)
        {
            entered = true;

            TriggerComportment(go);
        }
    }

    private void TriggerComportment(GameObject player)
    {
        DialogManager.Instance.AddMessage(new MedMsg("Try to escape this room!"));
    }
}

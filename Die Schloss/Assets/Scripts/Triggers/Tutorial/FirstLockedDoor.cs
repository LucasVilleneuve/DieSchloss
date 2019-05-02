using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLockedDoor : MonoBehaviour
{
    private Door door;
    private PlayerStateMachine psm;

    private void Start()
    {
        door = GetComponent<Door>();
        GameObject playerGo = GameObject.FindGameObjectWithTag("Player");
        psm = playerGo.GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        if (door.isPlayerClose() && psm.IsPlayerInSelectingState())
        {
            if (door.isDoorLocked())
            {
                if (door.isOpenInputPressed() && door.TryToGetKey() == null)
                {
                    DialogManager.Instance.AddMessage(new MedMsg("Try to find the key."));
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLockedDoor : MonoBehaviour
{
    [SerializeField] private EncounterMonster cutscene;

    private Door door;
    private PlayerStateMachine psm;
    private bool cutscenePlayed = false;

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
                if (door.isOpenInputPressed())
                {
                    if (door.TryToGetKey() == null)
                    {
                        ActionWhenLocked();
                    }
                }
            }
            else if (!cutscenePlayed)
            {
                cutscenePlayed = true;
                ActionWhenUnLocked();
            }  
        }
    }

    private void ActionWhenLocked()
    {
        DialogManager.Instance.AddMessage(new MedMsg("Try to find the key."));
    }

    private void ActionWhenUnLocked()
    {
        cutscene.StartCutscene();
    }
}

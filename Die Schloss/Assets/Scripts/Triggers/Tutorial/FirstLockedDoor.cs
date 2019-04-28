using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLockedDoor : Door
{
    private new void Update()
    {
        base.Update();
        if (playerClose && psm.IsPlayerInSelectingState())
        {
            if (isLocked)
            {
                if (openInputPressed && TryToGetKey() == null)
                {
                    DialogManager.Instance.AddMessage(new MedMsg("Try to find the key."));
                }
            }
        }
    }
}

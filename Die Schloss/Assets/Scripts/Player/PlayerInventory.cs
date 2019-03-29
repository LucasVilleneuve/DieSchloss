using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<UsableObject> inventory; // DEBUG

    public void Add(UsableObject obj)
    {
        inventory.Add(obj);
    }

    public void Remove(UsableObject obj)
    {
        if (obj != null)
            inventory.Remove(obj);
    }

    public void Remove(uint id)
    {
        Remove(Get(id));
    }

    public UsableObject Get(uint id)
    {
        return inventory.Find(obj => (obj.id == id));
    }
}
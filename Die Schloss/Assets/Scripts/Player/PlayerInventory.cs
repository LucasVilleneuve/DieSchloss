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
        Remove(Get((int)id));
    }

    /// <summary>
    /// Returns the object from player inventory.
    /// </summary>
    /// <param name="id">Id of the object to find.</param>
    /// <returns>Expected object if found, otherwise returns null.</returns>
    public UsableObject Get(int id)
    {
        UsableObject ret = inventory.Find(obj => obj.id == id);
        if (ret == null)
        {

            Debug.Log("Returning " + ret);
            Debug.Log("Returning null");
        }

        return ret;
    }

    public override string ToString()
    { 
        string ret = "";
        foreach (UsableObject obj in inventory)
        {
            Debug.Log("Obj is null = " + (obj == null));
            ret += obj.ToString() + '\n';
        }
        return ret;
    }
}
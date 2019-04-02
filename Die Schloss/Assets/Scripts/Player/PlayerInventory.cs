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
        if (obj is null)
            return;
        inventory.Remove(obj);
    }

    public void Remove(int id)
    {
        UsableObject ret = Get(id);
        if (ret is null) return;
        Remove(ret);
    }

    /// <summary>
    /// Returns the object from player inventory.
    /// </summary>
    /// <param name="id">Id of the object to find.</param>
    /// <returns>Expected object if found, otherwise returns null.</returns>
    public UsableObject Get(int id)
    {
        UsableObject ret = inventory.Find(obj => obj.id == id);
        return ret;
    }

    public override string ToString()
    { 
        string ret = "";
        foreach (UsableObject obj in inventory)
        {
            ret += obj.ToString() + '\n';
        }
        return ret;
    }
}
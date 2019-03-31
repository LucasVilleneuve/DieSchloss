using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableObject : MonoBehaviour
{
    public int id = -1;
    public string objName = "";
    public Sprite sprite = null;

    public UsableObject(uint id, string name)
    {
        this.id = (int)id;
        this.objName = name;
    }

    public virtual void Use()
    {
    }

    public override string ToString()
    {
        return "[" + id + "] " + objName;
    }
}
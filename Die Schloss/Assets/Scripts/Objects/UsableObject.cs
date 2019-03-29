using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UsableObject : MonoBehaviour
{
    public uint id;
    public string objName;
    public Sprite sprite;

    public UsableObject(uint id, string name)
    {
        this.id = id;
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
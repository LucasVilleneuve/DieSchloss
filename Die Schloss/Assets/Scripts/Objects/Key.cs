using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : UsableObject
{
    public Key(uint id, string name)
        : base(id, name)
    {
    }

    public override void Use()
    {
        Debug.Log("Using Key (" + id + " - " + objName + ")");
    }
}
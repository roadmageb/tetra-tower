using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdWoodenKeyChain : Addon, IAtkKeyMult
{
    public AdWoodenKeyChain(ScriptableAddonInfo _info) : base(_info)
    {
    }

    public float AtkKeyMult(float key, int skillNum, Enemy enemy)
    {
        return 1.3f;
    }
}

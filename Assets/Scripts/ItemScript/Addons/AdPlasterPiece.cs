using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdPlasterPiece : Addon, IAtkDmgAdd
{
    public AdPlasterPiece(ScriptableAddonInfo _info) : base(_info)
    {
    }

    public float AtkDmgAdd(AttackPtoE attack, int skillNum, Enemy enemy)
    {
        return 1;
    }
}

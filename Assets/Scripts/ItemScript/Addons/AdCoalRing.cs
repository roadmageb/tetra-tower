using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdCoalRing : Addon, IAtkDmgMult
{
    public AdCoalRing(ScriptableAddonInfo _info) : base(_info)
    {
    }

    public float AtkDmgMult(AttackPtoE attack, int skillNum, Enemy enemy)
    {
        return enemy.enemyCtrl.isBurn ? 1.3f : 1f;
    }
}

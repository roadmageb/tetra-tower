using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdCoolingStone : Addon, IAtkCtrl
{
    public AdCoolingStone(ScriptableAddonInfo _info) : base(_info)
    {
    }

    public void AtkCtrl(AttackPtoE attack, int skillNum, Enemy enemy)
    {
        if(Random.Range(0f, 1f) <= 0.12f)
        {
            attack.AddCtrl(CtrlPtoE.Freeze, 0f);
        }
    }
}

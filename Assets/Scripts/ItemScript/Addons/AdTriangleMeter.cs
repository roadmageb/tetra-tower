
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdTriangleMeter : Addon, IAtkSkillUse, IAtkDmgMult
{
    public bool trigger = false;
    public AdTriangleMeter(ScriptableAddonInfo _info) : base(_info)
    {
        trigger = false;
    }

    public float AtkDmgMult(AttackPtoE attack, int skillNum, Enemy enemy)
    {
        return trigger ? 1.5f : 1f;
    }

    public void AtkSkillUse(int skillNum)
    {
        if(gaugeSize == gaugeCurrent)
        {
            trigger = true;
            gaugeCurrent = 0;
        }
        else
        {
            trigger = false;
            gaugeCurrent++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdSpikeInsole : Addon, IAtkDmgMult
{
    public AdSpikeInsole(ScriptableAddonInfo _info) : base(_info)
    {
    }

    public float AtkDmgMult(AttackPtoE attack, int skillNum, Enemy enemy)
    {
        return wp.info.commands[skillNum].comboArrow == InputArrow.Down ? 1.4f : 1f;
    }
}

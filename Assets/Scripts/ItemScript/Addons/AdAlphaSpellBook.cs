using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdAlphaSpellBook : Addon, IAtkDmgMult
{
    public AdAlphaSpellBook(ScriptableAddonInfo _info) : base(_info)
    {
    }

    public float AtkDmgMult(AttackPtoE attack, int skillNum, Enemy enemy)
    {
        ComboInfo tmp = wp.info.commands[skillNum];
        return (tmp.comboAction[tmp.comboAction.Length - 1] | 1) == 1 ? 2f : 1f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public ItemRank rank;
    public int skillCount;
    public ComboInfo[] commands;
    public int damage;
    public bool gaugeEnabled;
    public float gaugeSize;
    public float gaugeCurrent;
    public int addonSize;
    public List<Addon> addons;

    public Weapon()
    {
        addons = new List<Addon>();
    }

    public bool GainAddon(Addon newAddon)
    {
        if(addons.Count < addonSize)
        {
            addons.Add(newAddon);
            return true;
        }
        return false;
    }

    public bool LoseAddon(Addon addon)
    {
        if(addons.Contains(addon))
        {
            addons.Remove(addon);
            return true;
        }
        return false;
    }

    public AttackPtoE CalcAttack(int skillNum, Enemy enemy)
    {
        if(skillCount > skillNum)
        {
            AttackPtoE attack = new AttackPtoE(damage);
            
            //Apply Ctrl effect
            if(this is IAtkCtrl)
            {
                ((IAtkCtrl)this).AtkCtrl(attack, skillNum, enemy);
            }
            foreach(Addon ad in addons)
            {
                if(ad is IAtkCtrl)
                {
                    ((IAtkCtrl)this).AtkCtrl(attack, skillNum, enemy);
                }
            }

            //Apply DmgAdd effect
            if (this is IAtkDmgAdd)
            {
                attack.damage += ((IAtkDmgAdd)this).AtkDmgAdd(attack, skillNum, enemy);
            }
            foreach (Addon ad in addons)
            {
                if (ad is IAtkDmgAdd)
                {
                    attack.damage += ((IAtkDmgAdd)ad).AtkDmgAdd(attack, skillNum, enemy);
                }
            }

            //Apply DmgAdd effect
            if (this is IAtkDmgMult)
            {
                ((IAtkDmgMult)this).AtkDmgMult(attack, skillNum, enemy);
            }
            foreach (Addon ad in addons)
            {
                if (ad is IAtkDmgMult)
                {
                    ((IAtkDmgMult)ad).AtkDmgMult(attack, skillNum, enemy);
                }
            }
            return attack;
        }
        return null;
    }
}

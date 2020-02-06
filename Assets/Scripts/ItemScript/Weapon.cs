using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
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
        }
        return null;
    }
}

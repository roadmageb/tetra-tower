using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public ScriptableWeaponInfo info;
    public List<Addon> addons;

    public Weapon(ScriptableWeaponInfo info)
    {
        addons = new List<Addon>();
        this.info = info;
    }

    public AnimationClip GetAnim(int skillNum)
    {
        return info.anims[skillNum];
    }
    public virtual void PlaySkill(int skillNum, int option)
    {

    }
    public bool GainAddon(Addon newAddon)
    {
        if(addons.Count < info.addonSize)
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
    protected virtual int GetDamage(int skillNum)
    {
        return 0;
    }
    public AttackPtoE CalcAttack(int skillNum, Enemy enemy)
    {
        if(info.skillCount > skillNum)
        {
            AttackPtoE attack = new AttackPtoE(GetDamage(skillNum));
            
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

            //Apply DmgMult effect
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

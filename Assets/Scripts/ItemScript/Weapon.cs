using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public ScriptableWeaponInfo info;
    public List<Addon> addons;
    public float gaugeSize;
    public float gaugeCurrent;

    public Weapon(ScriptableWeaponInfo _info)
    {
        addons = new List<Addon>();
        info = _info;
        for(int i = 0; i < info.commands.Length; i++)
        {
            info.commands[i].skill = new SkillInfo(this, i);
        }
        if(info.gaugeEnabled)
        {
            gaugeSize = info.gaugeSize;
            gaugeCurrent = info.gaugeInit;
        }
    }

    public AnimationClip GetAnim(int skillNum)
    {
        return info.commands[skillNum].anim;
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
    protected virtual float GetDamage(int skillNum)
    {
        return info.commands[skillNum].damageList[0];
    }

    public void ExecuteAttack(int skillNum, Enemy enemy, Transform attacker)
    {
        AttackPtoE atk;
        enemy.GainAttack(atk = CalcAttack(skillNum, enemy));
        if (this is IAtkOnHit)
        {
            ((IAtkOnHit)this).AtkOnHit(atk, attacker, skillNum, enemy);
        }
        foreach (Addon ad in addons)
        {
            if (ad is IAtkOnHit)
            {
                ((IAtkOnHit)this).AtkOnHit(atk, attacker, skillNum, enemy);
            }
        }
        PlayerController.Instance.GainKey(enemy.GiveKey(CalcKey(skillNum, enemy)));
    }

    public float CalcKey(int skillNum, Enemy enemy)
    {
        float key = info.commands[skillNum].keyGain;
        float tmpKey = key;
        if (this is IAtkKeyAdd)
        {
            tmpKey += ((IAtkKeyAdd)this).AtkKeyAdd(key, skillNum, enemy);
        }
        foreach (Addon ad in addons)
        {
            if (ad is IAtkKeyAdd)
            {
                tmpKey += ((IAtkKeyAdd)this).AtkKeyAdd(key, skillNum, enemy);
            }
        }
        if (this is IAtkKeyMult)
        {
            tmpKey *= ((IAtkKeyMult)this).AtkKeyMult(key, skillNum, enemy);
        }
        foreach (Addon ad in addons)
        {
            if (ad is IAtkKeyMult)
            {
                tmpKey *= ((IAtkKeyMult)this).AtkKeyMult(key, skillNum, enemy);
            }
        }
        return tmpKey;
    }

    public AttackPtoE CalcAttack(int skillNum, Enemy enemy)
    {
        if(info.commands.Length > skillNum)
        {
            AttackPtoE attack = new AttackPtoE(GetDamage(skillNum));
            float tmpDmg = attack.damage;
            
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
                tmpDmg += ((IAtkDmgAdd)this).AtkDmgAdd(attack, skillNum, enemy);
            }
            foreach (Addon ad in addons)
            {
                if (ad is IAtkDmgAdd)
                {
                    tmpDmg += ((IAtkDmgAdd)ad).AtkDmgAdd(attack, skillNum, enemy);
                }
            }

            //Apply DmgMult effect
            if (this is IAtkDmgMult)
            {
                tmpDmg *= ((IAtkDmgMult)this).AtkDmgMult(attack, skillNum, enemy);
            }
            foreach (Addon ad in addons)
            {
                if (ad is IAtkDmgMult)
                {
                    tmpDmg *= ((IAtkDmgMult)ad).AtkDmgMult(attack, skillNum, enemy);
                }
            }
            attack.damage = tmpDmg;
            return attack;
        }
        return null;
    }
}

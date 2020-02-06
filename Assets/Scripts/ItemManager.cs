using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public List<Weapon> weapons;

    private void Start()
    {
        weapons = new List<Weapon>();
    }

    /// <summary>
    /// Check if there are some duplicate commands between chkWeapon and weapons that player possesses
    /// Returns list of duplicated weapons
    /// </summary>
    /// <param name="chkWeapon"></param>
    /// <returns></returns>
    public List<Weapon> ComboDuplicateCheck(Weapon chkWeapon)
    {
        List<Weapon> duplicateWeapons = new List<Weapon>();

        foreach(Weapon wp in weapons)
        {
            bool dupChk = false;
            foreach(ComboInfo ci in chkWeapon.commands)
            {
                foreach(ComboInfo cj in wp.commands)
                {
                    dupChk |= ci.CheckEqualCombo(cj);
                }
            }
            if(dupChk)
            {
                duplicateWeapons.Add(wp);
            }
        }
        return duplicateWeapons;
    }
    
    public bool GainWeapon(Weapon wp)
    {
        if(ComboDuplicateCheck(wp).Count == 0)
        {
            weapons.Add(wp);
            return true;
        }
        return false;
    }

    public bool LoseWeapon(Weapon wp)
    {
        if(weapons.Contains(wp))
        {
            weapons.Remove(wp);
            return true;
        }
        return false;
    }
}

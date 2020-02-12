using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public List<Weapon> weapons;
    public List<Weapon> weaponDB;

    void test()
    {
        GainWeapon(weaponDB[0]);
    }
    private void Start()
    {
        weapons = new List<Weapon>();
        weaponDB = new List<Weapon>()
        {
            new WpStick()
        };
        test();
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
        if(weapons.Count < 9 && ComboDuplicateCheck(wp).Count == 0)
        {
            weapons.Add(wp);
            PlayerController.Instance.ResetPossibleComboes();
            return true;
        }
        return false;
    }

    public bool LoseWeapon(Weapon wp)
    {
        if(weapons.Contains(wp))
        {
            weapons.Remove(wp);
            PlayerController.Instance.ResetPossibleComboes();
            return true;
        }
        return false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] private DroppedItem droppedItem = null;
    public GameObject droppedLifeStone;
    public List<Weapon> currentWeapons;
    public ScriptableWeaponInfo[] weaponDB;
    public List<ScriptableWeaponInfo>[] weaponRankList;

    [SerializeField] private bool isTest;

    void test()
    {
        GainWeapon(InstantiateWeapon(ItemRank.Monomino));
        GainWeapon(InstantiateWeapon(ItemRank.Domino));
        GainWeapon(InstantiateWeapon(ItemRank.Domino));
    }
    private void Start()
    {
        currentWeapons = new List<Weapon>();
        weaponRankList = new List<ScriptableWeaponInfo>[Enum.GetNames(typeof(ItemRank)).Length];
        for(int i = 0; i < weaponRankList.Length; i++)
        {
            weaponRankList[i] = new List<ScriptableWeaponInfo>();
        }
        foreach (ScriptableWeaponInfo info in weaponDB)
        {
            weaponRankList[(int)info.rank].Add(info);
        }
        if(isTest) test();
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

        foreach(Weapon wp in currentWeapons)
        {
            bool dupChk = false;
            foreach(ComboInfo ci in chkWeapon.info.commands)
            {
                foreach(ComboInfo cj in wp.info.commands)
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
    public Weapon InstantiateWeapon(ItemRank rank)
    {
        if (weaponRankList[(int)rank].Count > 0)
        {
            int index = Random.Range(0, weaponRankList[(int)rank].Count);
            ScriptableWeaponInfo info = weaponRankList[(int)rank][index];
            weaponRankList[(int)rank].RemoveAt(index);
            return (Weapon)Activator.CreateInstance(Type.GetType(info.name), new object[] { info });
        }
        return null;
    }

    public Weapon InstantiateWeapon(string name)
    {
        for (int i = 0; i < weaponRankList.Length; i++)
        {
            foreach (ScriptableWeaponInfo info in weaponRankList[i])
            {
                if(info.name == name)
                {
                    return (Weapon)Activator.CreateInstance(Type.GetType(info.name), new object[] { info });
                }
            }
        }
        return null;
    }

    public bool GainWeapon(Weapon wp)
    {
        if(currentWeapons.Count < 9 && ComboDuplicateCheck(wp).Count == 0)
        {
            currentWeapons.Add(wp);
            PlayerController.Instance.ResetPossibleComboes();
            return true;
        }
        return false;
    }

    public bool LoseWeapon(Weapon wp)
    {
        if(currentWeapons.Contains(wp))
        {
            currentWeapons.Remove(wp);
            PlayerController.Instance.ResetPossibleComboes();
            return true;
        }
        return false;
    }

    public DroppedItem CreateItem(Vector2 pos, int lifeStoneAmount, int goldRate)
    {
        float droppedLifeStoneOffset = 0.33f;
        DroppedItem temp = Instantiate(droppedItem, pos, Quaternion.identity);
        LifeStoneInfo lifeStoneInfo = LifeStoneManager.Instance.CreateLifeStoneShape(lifeStoneAmount, goldRate);
        temp.lifeStoneInfo = lifeStoneInfo;

        for (int y = 0; y < lifeStoneInfo.size.y; y++)
        {
            for (int x = 0; x < lifeStoneInfo.size.x; x++)
            {
                if((LifeStoneType)int.Parse(lifeStoneInfo.lifeStonePos[y * lifeStoneInfo.size.x + x].ToString()) != LifeStoneType.NULL)
                {
                    Instantiate(droppedLifeStone, new Vector2(x, y) * droppedLifeStoneOffset, Quaternion.identity, temp.transform);
                }
            }
        }
        temp.GetComponent<BoxCollider2D>().size = (Vector2)lifeStoneInfo.size * droppedLifeStoneOffset;
        temp.GetComponent<BoxCollider2D>().offset = (lifeStoneInfo.size - new Vector2(1, 1)) / 2 * droppedLifeStoneOffset;
        temp.transform.Find("GroundCollider").GetComponent<BoxCollider2D>().size = (Vector2)lifeStoneInfo.size * droppedLifeStoneOffset;
        temp.transform.Find("GroundCollider").GetComponent<BoxCollider2D>().offset = (lifeStoneInfo.size - new Vector2(1, 1)) / 2 * droppedLifeStoneOffset;

        temp.isWeapon = false;

        return temp;
    }

    public DroppedItem CreateItem(Vector2 pos, Weapon wp)
    {
        if(wp != null)
        {
            DroppedItem temp = Instantiate(droppedItem, pos, Quaternion.identity);
            temp.weapon = wp;
            temp.GetComponent<SpriteRenderer>().sprite = wp.info.sprite;
            temp.isWeapon = true;
            return temp;
        }
        else
        {
            return null;
        }
    }

    public DroppedItem CreateItem(Vector2 pos, ItemRank rank)
    {
        return CreateItem(pos, InstantiateWeapon(rank));
    }
    public DroppedItem CreateItem(Vector2 pos, string name)
    {
        return CreateItem(pos, InstantiateWeapon(name));
    }

}

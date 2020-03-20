using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] private DroppedItem droppedItem = null;
    public GameObject droppedLifeStone;
    [HideInInspector] public List<Weapon> currentWeapons;
    public ScriptableWeaponInfo[] weaponDB;
    [HideInInspector] public List<ScriptableWeaponInfo>[] weaponRankList;

    [HideInInspector] public List<Addon> currentAddons;
    public ScriptableAddonInfo[] addonDB;
    [HideInInspector] public List<ScriptableAddonInfo>[] addonRankList;

    [SerializeField] private bool isTest;

    [SerializeField] private Sprite[] lifeStoneSprites;
    private Sprite[] borderedLifeStoneSprites;

    void test()
    {
        CreateItem(Vector3.zero, ItemRank.Monomino, false);
        CreateItem(Vector3.zero, ItemRank.Monomino, false);
        CreateItem(Vector3.zero, ItemRank.Domino, false);
        CreateItem(Vector3.zero, ItemRank.Domino, false);
        //CreateItem(Vector3.zero, 3, 0);
        //GainWeapon(InstantiateWeapon(ItemRank.Monomino));
        //GainWeapon(InstantiateWeapon(ItemRank.Monomino));
        //GainWeapon(InstantiateWeapon(ItemRank.Domino));
        //GainWeapon(InstantiateWeapon(ItemRank.Domino));
    }
    private void Start()
    {
        borderedLifeStoneSprites = new Sprite[lifeStoneSprites.Length];
        for(int i = 0; i < lifeStoneSprites.Length; i++)
        {
            borderedLifeStoneSprites[i] = BorderSprite(lifeStoneSprites[i], 5);
        }

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

        currentAddons = new List<Addon>();
        addonRankList = new List<ScriptableAddonInfo>[Enum.GetNames(typeof(ItemRank)).Length];
        for (int i = 0; i < addonRankList.Length; i++)
        {
            addonRankList[i] = new List<ScriptableAddonInfo>();
        }
        foreach (ScriptableAddonInfo info in addonDB)
        {
            addonRankList[(int)info.rank].Add(info);
        }

        if (isTest) test();
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

        foreach (Weapon wp in currentWeapons)
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
    public Addon InstantiateAddon(ItemRank rank)
    {
        if (addonRankList[(int)rank].Count > 0)
        {
            int index = Random.Range(0, addonRankList[(int)rank].Count);
            ScriptableAddonInfo info = addonRankList[(int)rank][index];
            addonRankList[(int)rank].RemoveAt(index);
            return (Addon)Activator.CreateInstance(Type.GetType(info.name), new object[] { info });
        }
        return null;
    }

    public Addon InstantiateAddon(string name)
    {
        for (int i = 0; i < addonRankList.Length; i++)
        {
            foreach (ScriptableAddonInfo info in addonRankList[i])
            {
                if (info.name == name)
                {
                    return (Addon)Activator.CreateInstance(Type.GetType(info.name), new object[] { info });
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
            UIManager.Instance.AddWeaponToUI(wp);
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

    public bool GainAddon(Addon ad)
    {
        if (currentAddons.Count < 9)
        {
            currentAddons.Add(ad);
            return true;
        }
        return false;
    }

    public bool LoseAddon(Addon ad)
    {
        if (currentAddons.Contains(ad))
        {
            if(ad.wp != null)
            {
                ad.wp.LoseAddon(ad);
            }
            currentAddons.Remove(ad);
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
                    var obj = Instantiate(droppedLifeStone, new Vector2(x, y) * droppedLifeStoneOffset, Quaternion.identity, temp.transform);
                    obj.GetComponent<SpriteRenderer>().sprite = borderedLifeStoneSprites[0];
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

    public Sprite BorderSprite(Sprite sprt, int border)
    {
        Texture2D tex = sprt.texture;
        Texture2D ret = new Texture2D(tex.width + border * 2, tex.height + border * 2);
        Color[] c = new Color[(tex.width + border * 2) * (tex.height + border * 2)];
        for (int i = 0; i < c.Length; i++)
        {
            c[i] = Color.clear;
        }
        
        ret.SetPixels(c);
        ret.SetPixels(border, border, tex.width, tex.height, tex.GetPixels());
        ret.filterMode = FilterMode.Point;
        ret.Apply();
        return Sprite.Create(ret, new Rect(0, 0, tex.width + border * 2, tex.height + border * 2), new Vector2(0.5f, 0.5f), sprt.pixelsPerUnit);
    }

    public DroppedItem CreateItem(Vector2 pos, Weapon wp)
    {
        if(wp != null)
        {
            DroppedItem temp = Instantiate(droppedItem, pos, Quaternion.identity);
            temp.weapon = wp;
            temp.GetComponent<SpriteRenderer>().sprite = BorderSprite(wp.info.sprite, 10) ;
            temp.isWeapon = true;
            return temp;
        }
        else
        {
            return null;
        }
    }
    public DroppedItem CreateItem(Vector2 pos, Addon ad)
    {
        if (ad != null)
        {
            //DroppedItem temp = Instantiate(droppedItem, pos, Quaternion.identity);
            //temp.weapon = ad;
            //temp.GetComponent<SpriteRenderer>().sprite = BorderSprite(wp.info.sprite.texture, 10);
            //temp.isWeapon = true;
            //return temp;
            return null;
        }
        else
        {
            return null;
        }
    }

    public DroppedItem CreateItem(Vector2 pos, ItemRank rank, bool isAddon = false)
    {
        return isAddon ? CreateItem(pos, InstantiateAddon(rank)) : CreateItem(pos, InstantiateWeapon(rank));
    }
    public DroppedItem CreateItem(Vector2 pos, string name)
    {
        Weapon wp = InstantiateWeapon(name);
        Addon ad = InstantiateAddon(name);
        if (wp != null)
        {
            return CreateItem(pos, InstantiateWeapon(name));
        }
        else if(ad != null)
        {
            return CreateItem(pos, InstantiateAddon(name));
        }
        return null;
    }

}

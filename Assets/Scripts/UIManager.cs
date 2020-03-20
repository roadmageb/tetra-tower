using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public int currentWeaponIndex = 0;
    public Transform lifeStoneUI, miniMap, itemUI;
    public GameObject[] itemIcons;
    public GameObject itemIcon;
    public Text keyCountText;

    public void ExpandWeaponUI(Weapon weapon)
    {
        itemIcons[currentWeaponIndex] = Instantiate(itemIcon, Vector3.zero, Quaternion.identity, itemUI);
        itemIcons[currentWeaponIndex].GetComponent<Image>().sprite = weapon.info.sprite;
        currentWeaponIndex++;
    }

    private void Awake()
    {
        itemIcons = new GameObject[9];
    }

    private void LateUpdate()
    {
        keyCountText.text = PlayerController.Instance.keyAmount.ToString();
    }
}

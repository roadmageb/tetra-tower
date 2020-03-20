using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public int currentWeaponIndex = 0;
    public Transform lifeStoneUI, miniMap, itemUI;
    public GameObject[] itemIcons;
    public Text keyCountText;

    public void AddWeaponToUI(Weapon weapon)
    {
        itemIcons[currentWeaponIndex].GetComponent<Image>().sprite = weapon.info.sprite;
        currentWeaponIndex++;
    }

    private void LateUpdate()
    {
        keyCountText.text = PlayerController.Instance.keyAmount.ToString();
    }
}

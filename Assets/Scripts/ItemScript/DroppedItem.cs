using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    private bool playerTouched = false;
    public Weapon weapon;
    public LifeStoneInfo lifeStoneInfo;
    public bool isWeapon = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            playerTouched = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            playerTouched = false;
        }
    }

    private void Update()
    {
        if (playerTouched && Input.GetKeyDown(KeyCode.E))
        {
            if (isWeapon)
            {
                ItemManager.Instance.GainWeapon(weapon);
            }
            else
            {
                LifeStoneManager.Instance.GetLifeStone(lifeStoneInfo);
            }
            Destroy(gameObject);
        }
    }
}

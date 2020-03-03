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
            GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 0.03f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            playerTouched = false;
            GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 0);
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

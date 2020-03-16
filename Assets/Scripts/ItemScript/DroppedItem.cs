using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    private bool playerTouched = false;
    public Weapon weapon;
    public Addon addon;
    public LifeStoneInfo lifeStoneInfo;
    public DroppedItemType droppedItemType;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            Vector3 playerPos = PlayerController.Instance.transform.position;
            if(PlayerController.Instance.closestDroppedItem == null || 
                (Vector3.Distance(playerPos, PlayerController.Instance.closestDroppedItem.transform.position) > Vector3.Distance(playerPos, transform.position)))
            {
                PlayerController.Instance.closestDroppedItem = gameObject;
                playerTouched = true;
                GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 0.03f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && PlayerController.Instance.closestDroppedItem == gameObject)
        {
            PlayerController.Instance.closestDroppedItem = null;
            playerTouched = false;
            GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 0);
        }
    }

    private void Update()
    {
        if (playerTouched && Input.GetKeyDown(KeyCode.E))
        {
            switch (droppedItemType)
            {
                case DroppedItemType.Weapon:
                    ItemManager.Instance.GainWeapon(weapon);
                    break;
                case DroppedItemType.Addon:
                    ItemManager.Instance.GainAddon(addon);
                    break;
                case DroppedItemType.LifeStone:
                    LifeStoneManager.Instance.GetLifeStone(lifeStoneInfo);
                    break;
            }
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = null;
        if(enemy = collision.GetComponent<Enemy>())
        {
            PlayerController.Instance.PlayerAttack(enemy);
        }
    }
}

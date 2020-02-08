using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public EnemyCtrl enemyCtrl;

    public void GainAttack(AttackPtoE attack)
    {
        currentHP -= attack.damage;
        if (currentHP <= 0)
        {
            Death();
        }
        enemyCtrl.ApplyCtrl(attack);
    }

    public void Death()
    {

    }
}

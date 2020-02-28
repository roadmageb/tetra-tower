using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatFly : Enemy
{
    [SerializeField] float jumpPower = 100;

    public void AttackPattern1()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            AttackEnd();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerAttackable && !attackedPlayer && collision.gameObject.tag.Equals("Player"))
        {
            attackedPlayer = true;
            PlayerController.Instance.GetDamage(attackPattern[currentAttackIndex].attackDamage);
        }
    }

    public override void IdleAction()
    {
        base.IdleAction();

    }

    protected override void Start()
    {
        base.Start();
        attackMethods.Add(AttackPattern1);
    }
}

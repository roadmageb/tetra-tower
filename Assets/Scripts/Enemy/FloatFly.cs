using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatFly : Enemy
{
    [SerializeField] private float flySpeed;

    public void AttackPattern1()
    {

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
        FlyRandomMovement();
    }

    public override void TraceAction()
    {
        base.TraceAction();
        FlyRandomMovement();
    }

    private void FlyRandomMovement()
    {
        rb.AddForce(Random.insideUnitSphere * flySpeed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    float attackStartTime, attackWaitTime = 2;
    [SerializeField] float jumpPower = 100;

    public override void SeekTarget()
    {
        base.SeekTarget();
    }

    public override void Attack()
    {
        StartCoroutine(SlimeAttack());
    }

    private IEnumerator SlimeAttack()
    {
        yield return new WaitForSeconds(1);
        Vector2 direction = new Vector2(target.position.x - transform.position.x > 0 ? 1 : -1, 0.5f);
        rb.AddForce(direction * jumpPower);
    }
}

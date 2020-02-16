using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    float attackStartTime, attackWaitTime = 2;
    [SerializeField] float jumpPower = 100;
    [SerializeField] bool isFloat = false;

    public override void SeekTarget()
    {
        base.SeekTarget();
    }

    public override void Attack()
    {
        Vector2 direction = isFloat ? (Vector2)(target.position - transform.position).normalized: new Vector2(target.position.x - transform.position.x > 0 ? 1 : -1, 0);
        rb.AddForce(direction * jumpPower);
    }

    IEnumerator SlimeUpCoroutine()
    {
        for(float timer = 0; timer < 1; timer += Time.deltaTime)
        {
            yield return null;
            transform.position += new Vector3(0, 0.002f);
        }
    }

    public void SlimeAttackStart()
    {
        StartCoroutine(SlimeUpCoroutine());
    }
}

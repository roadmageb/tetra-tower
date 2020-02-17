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

    public void AttackPattern1()
    {
        StartCoroutine(SlimeAttackWaitCoroutine());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            AttackEnd();
        }
    }

    IEnumerator SlimeAttackWaitCoroutine()
    {
        StartCoroutine(SlimeUpCoroutine());
        yield return new WaitForSeconds(1.5f);
        if (isFloat)
        {
            Vector2 targetPos = target.position;
            Vector2 thisPos = transform.position;
            targetPos.x = targetPos.x - thisPos.x;
            targetPos.y = targetPos.y - thisPos.y;
            float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.localScale.x > 0 ? angle : angle + 180));
        }
        Vector2 direction = isFloat ? (Vector2)(target.position - transform.position).normalized : new Vector2(target.position.x - transform.position.x > 0 ? 1 : -1, 0);
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

    protected override void Start()
    {
        base.Start();
        attackMethods.Add(AttackPattern1);
    }
}

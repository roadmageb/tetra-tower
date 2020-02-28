using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatFly : Enemy
{
    float attackStartTime, attackWaitTime = 2;
    [SerializeField] float jumpPower = 100;
    [SerializeField] bool isFloat = false;

    public void AttackPattern1()
    {
        StartCoroutine(SlimeAttackWaitCoroutine());
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

    IEnumerator SlimeAttackWaitCoroutine()
    {
        StartCoroutine(SlimeUpCoroutine());
        yield return new WaitForSeconds(1.5f);
        playerAttackable = true;
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
        for (float timer = 0; timer < 1; timer += Time.deltaTime)
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

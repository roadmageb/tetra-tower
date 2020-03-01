using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] float jumpPower = 100;
    [SerializeField] bool isFloat = false;

    public void AttackPattern1()
    {
        playerAttackable = true;
        Vector2 direction = isFloat ? (Vector2)(target.position - transform.position).normalized : new Vector2(target.position.x - transform.position.x > 0 ? 1 : -1, 0);
        rb.AddForce(direction * jumpPower);
    }

    public void SlimeUp()
    {
        rb.AddForce(new Vector2(0, 500));
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
}

﻿using System.Collections;
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

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.tag.Equals("Floor"))
        {
            AttackEnd();
        }
    }
}

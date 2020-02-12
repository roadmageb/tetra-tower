using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    float attackStartTime, attackWaitTime = 2;
    bool startCharge = false;
    [SerializeField] float jumpPower = 100;

    public override bool SeekTarget()
    {
        return base.SeekTarget();
    }

    public override void Attack()
    {
        Vector2 direction = new Vector2(target.position.x - transform.position.x > 0 ? 1 : -1, 0.5f);

        rb.AddForce(direction * jumpPower);
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.Raycast(rb.position, Vector3.right * (transform.localScale.x > 0 ? 1 : -1), playerDetectDistance, LayerMask.GetMask("Player")))
        {
            animator.SetBool("Trace", true);
        }
        //SeekTarget();
    }
}

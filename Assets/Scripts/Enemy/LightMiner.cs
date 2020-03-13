using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMiner : Enemy
{
    [SerializeField] private GameObject floatFly;

    public void AttackPattern1()
    {
        Instantiate(floatFly, transform.position + new Vector3(0, 1, 0), Quaternion.identity, transform);
        Instantiate(floatFly, transform.position + new Vector3(0, 1, 0), Quaternion.identity, transform);
        Instantiate(floatFly, transform.position + new Vector3(0, 1, 0), Quaternion.identity, transform);
    }

    public override void TraceAction()
    {
        Patrol();
        if (Time.time - lastAttackedTime > attackDelay && Vector3.Distance(rb.position, target.position) <= attackRange)
        {
            AttackStart();
        }
    }
    public override void AttackAction()
    {
        base.AttackAction();
        Patrol();
    }
}

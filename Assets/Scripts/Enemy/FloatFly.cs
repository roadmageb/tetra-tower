using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatFly : Enemy
{
    [SerializeField] private float flySpeed;

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

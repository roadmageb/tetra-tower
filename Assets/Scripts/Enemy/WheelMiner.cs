using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMiner : Enemy
{
    public override void IdleAction()
    {
        base.IdleAction();
        Patrol();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMiner : Enemy
{
    [SerializeField] private GameObject floatFly;

    public void AttackPattern1()
    {
        Instantiate(floatFly, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    }
}

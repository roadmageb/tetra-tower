using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMiner : Enemy
{
    [SerializeField] private GameObject floatFly;
    private GameObject lightMinerAlert;

    public void AttackPattern1()
    {
        if(transform.GetComponentsInChildren<FloatFly>().Length == 0)
        {
            Instantiate(floatFly, transform.position + new Vector3(0, 1, 0), Quaternion.identity, transform);
            Instantiate(floatFly, transform.position + new Vector3(0, 1, 0), Quaternion.identity, transform);
            Instantiate(floatFly, transform.position + new Vector3(0, 1, 0), Quaternion.identity, transform);
        }
    }

    protected override void Start()
    {
        base.Start();
        lightMinerAlert = transform.Find("LightMinerAlert").gameObject;
    }

    protected override void Update()
    {
        base.Update();
        lightMinerAlert.SetActive(DetectPlayer());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPtoE
{
    public float damage;
    public List<KeyValuePair<CtrlPtoE, float>> ctrls;

    public AttackPtoE(float dmg)
    {
        damage = dmg;
    }

    public void AddCtrl(CtrlPtoE ctrl, float f)
    {
        ctrls.Add(new KeyValuePair<CtrlPtoE, float>(ctrl, f));
    }
}

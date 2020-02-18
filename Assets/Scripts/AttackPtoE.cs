using System.Collections;
using System.Collections.Generic;

public class AttackPtoE
{
    public float damage;
    public Dictionary<CtrlPtoE, float> ctrls;

    public AttackPtoE(float dmg)
    {
        damage = dmg;
        ctrls = new Dictionary<CtrlPtoE, float>();
    }

    public void AddCtrl(CtrlPtoE ctrl, float f)
    {
        if (ctrls.ContainsKey(ctrl))
        {
            ctrls[ctrl] += f;
        }
        else
        {
            ctrls.Add(ctrl, f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WpSharpDagger : Weapon
{
    public WpSharpDagger(ScriptableWeaponInfo info) : base(info)
    {
    }
    public override void PlaySkill(int skillNum, int option)
    {
        Transform trns = PlayerController.Instance.transform;
        switch (skillNum)
        {
            case 0:
                Vector3 stPos, enPos;
                stPos = trns.position;
                enPos = stPos + new Vector3(trns.localScale.x * 3, 0, 0);
                GameObject obj = Object.Instantiate(info.projectiles[0]);
                obj.GetComponent<ProjSharpDagger>().SetProjectile(
                    new SkillInfo(this, 0),
                    (stPos + enPos) / 2 + new Vector3(0, 1, 0),
                    (int)trns.localScale.x);
                trns.position = enPos;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WpNeedleAndThread : Weapon
{
    public WpNeedleAndThread(ScriptableWeaponInfo info) : base(info)
    {
    }

    public override void PlaySkill(int skillNum, int option)
    {
        Transform trns = PlayerController.Instance.transform;
        switch (skillNum)
        {
            case 0:
                Object.Instantiate(info.projectiles[0]).GetComponent<ProjBow>().SetProjectile(
                    new SkillInfo(this, 0),
                    trns.position + new Vector3(1, 1, 0),
                    false,
                    (int)trns.localScale.x);
                break;
            case 1:
                break;
        }
    }
}

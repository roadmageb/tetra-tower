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
        Rigidbody2D rb2d = PlayerController.Instance.GetComponent<Rigidbody2D>();
        switch (skillNum)
        {
            case 0:
                Vector3 stPos, enPos;
                RaycastHit2D hit;
                float originDistance = 3f, alterDistance;

                stPos = trns.position;
                hit = Physics2D.Raycast(PlayerController.Instance.controller.m_WallCheck.position, Vector2.right * trns.localScale.x, originDistance, 1 << LayerMask.NameToLayer("Floor"));
                alterDistance = !hit ? originDistance : hit.distance;
                enPos = stPos + new Vector3(trns.localScale.x * alterDistance, 0, 0);
                GameObject obj = Object.Instantiate(info.projectiles[0]);
                obj.GetComponent<ProjSharpDagger>().SetProjectile(
                    new SkillInfo(this, 0),
                    (stPos + enPos) / 2 + PlayerController.Instance.controller.m_PlayerCenter.position - trns.position,
                    (int)trns.localScale.x * alterDistance / originDistance);
                trns.position = enPos;

                rb2d.gravityScale = 0;
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);

                break;
        }
    }
}

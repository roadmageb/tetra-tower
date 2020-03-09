using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WpBWSpear : Weapon
{
    public WpBWSpear(ScriptableWeaponInfo info) : base(info)
    {
    }

    public override void PlaySkill(int skillNum, int option)
    {
        Transform trns = PlayerController.Instance.transform;
        Rigidbody2D rb2D = PlayerController.Instance.GetComponent<Rigidbody2D>();
        switch (skillNum)
        {
            case 0:
                EffectPool.Instance.StartEffect(GameManager.Instance.effectAnim["BWSpear00"], trns.position + Vector3.Scale(new Vector3(2.5f, 1, 0), trns.localScale), trns.localScale);
                if (PlayerController.Instance.controller.m_Grounded)
                {
                    rb2D.velocity = new Vector2(0, 0);
                }
                else
                {
                    rb2D.velocity = new Vector2(0, 0);
                    rb2D.gravityScale = 0;
                }
                break;
            case 1:
                EffectPool.Instance.StartEffect(GameManager.Instance.effectAnim["BWSpear01"], trns.position + Vector3.Scale(new Vector3(2.5f, 1, 0), trns.localScale), trns.localScale);
                if (PlayerController.Instance.controller.m_Grounded)
                {
                    rb2D.velocity = new Vector2(0, 0);
                }
                else
                {
                    rb2D.velocity = new Vector2(0, 0);
                    rb2D.gravityScale = 0;
                }
                break;
        }
    }
}

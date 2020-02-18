using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WpBWSpear : Weapon
{
    public WpBWSpear()
    {
        rank = ItemRank.Domino;
        skillCount = 2;
        commands = new ComboInfo[]
        {
            new ComboInfo(new SkillInfo(this, 0), InputArrow.Front, new int[]{2}, 3, 18, "1타"),
            new ComboInfo(new SkillInfo(this, 1), InputArrow.Front, new int[]{2, 4}, 10, 30, "2타")
        };
        gaugeEnabled = false;
        addonSize = 2;
        anims = new AnimationClip[]
        {
            GameManager.Instance.skillAnim["BWSpear00"],
            GameManager.Instance.skillAnim["BWSpear01"]
        };
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
                    rb2D.velocity = new Vector2(0, 8);
                }
                break;
        }
    }
}

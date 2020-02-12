using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WpStick : Weapon
{
    public WpStick()
    {
        rank = ItemRank.Monomino;
        skillCount = 1;
        commands = new ComboInfo[]
        {
            new ComboInfo(new SkillInfo(this, 0), InputArrow.NULL, new int[]{1}, 5, 18, "타격")
        };
        gaugeEnabled = false;
        addonSize = 3;
        anims = new AnimationClip[]
        {
            GameManager.Instance.skillAnim["Stick00"]
        };
    }
    public override void PlaySkill(int skillNum, int option)
    {
        Transform trns = PlayerController.Instance.transform;
        switch(skillNum)
        {
            case 0:
                EffectPool.Instance.StartEffect(GameManager.Instance.effectAnim["Stick00"], trns.position + Vector3.Scale(new Vector3(1, 0, 0), trns.localScale), trns.localScale);
                break;
        }
    }
}

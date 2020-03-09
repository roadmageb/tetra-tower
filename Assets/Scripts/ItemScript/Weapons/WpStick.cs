using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WpStick : Weapon
{
    public WpStick(ScriptableWeaponInfo info) : base(info)
    {

    }

    public override void PlaySkill(int skillNum, int option)
    {
        Transform trns = PlayerController.Instance.transform;
        switch(skillNum)
        {
            case 0:
                EffectPool.Instance.StartEffect(GameManager.Instance.effectAnim["Stick00"], trns.position + Vector3.Scale(new Vector3(1, 1, 0), trns.localScale), trns.localScale);
                if(PlayerController.Instance.controller.m_Grounded)
                {
                    PlayerController.Instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                }
                else
                {
                    PlayerController.Instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 8);
                }
                break;
        }
    }
}

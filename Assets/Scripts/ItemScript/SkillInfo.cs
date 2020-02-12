using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo
{
    public Weapon wp;
    public int num;
    public SkillInfo(Weapon weapon, int skillNum)
    {
        wp = weapon;
        num = skillNum;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public ItemRank rank;
    public SkillCommand[] commands;
    public int skillCount;
    public AnimationClip[] skillAnim;
    public int damage;
    public bool gaugeEnabled;
    public float gaugeSize;
    public float gaugeCurrent;
    public int addonSize;
    public Addon[] addons;
}

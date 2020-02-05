using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public ItemRank rank;
    public int skillCount;
    public ComboInfo[] commands;
    public AnimationClip[] skillAnim;
    public float[] skillDelay;
    public int damage;
    public bool gaugeEnabled;
    public float gaugeSize;
    public float gaugeCurrent;
    public int addonSize;
    public Addon[] addons;


}

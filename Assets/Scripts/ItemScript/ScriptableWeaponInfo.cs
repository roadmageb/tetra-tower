using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableWeaponInfo : ScriptableObject
{
    public ItemRank rank;
    public int skillCount;
    public ComboInfo[] commands;
    public float[] damageList;
    public AnimationClip[] anims;
    public bool gaugeEnabled;
    public float gaugeSize;
    public float gaugeCurrent;
    public int addonSize;
}

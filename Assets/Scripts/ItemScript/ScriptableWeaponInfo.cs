using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Custom/WeaponInfo")]
public class ScriptableWeaponInfo : ScriptableObject
{
    public ItemRank rank;
    public ComboInfo[] commands;
    public bool gaugeEnabled;
    public float gaugeSize;
    public float gaugeInit;
    public int addonSize;
    public Sprite sprite;
    public GameObject[] projectiles;
    public AnimationClip[] clips;
}

/*
[CustomEditor(typeof(ScriptableWeaponInfo))]
public class MyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var obj = target as ScriptableWeaponInfo;
    }
}
*/
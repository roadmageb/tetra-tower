using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAddon", menuName = "Custom/AddonInfo")]
public class ScriptableAddonInfo : ScriptableObject
{
    public ItemRank rank;
    public bool gaugeEnabled;
    public float gaugeSize;
    public float gaugeInit;
    public Sprite sprite;
}

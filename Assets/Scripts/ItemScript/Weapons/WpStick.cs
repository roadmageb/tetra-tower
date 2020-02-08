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
            new ComboInfo(InputArrow.NULL, new int[]{1}, 5, 18, "타격")
        };
        gaugeEnabled = false;
        addonSize = 3;
    }

}

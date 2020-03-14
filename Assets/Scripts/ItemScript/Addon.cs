using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Addon
{
    public Weapon wp;
    public ScriptableAddonInfo info;
    public float gaugeSize;
    public float gaugeCurrent;

    public Addon(ScriptableAddonInfo _info)
    {
        wp = null;
        info = _info;
        if (info.gaugeEnabled)
        {
            gaugeSize = info.gaugeSize;
            gaugeCurrent = info.gaugeInit;
        }
    }
}

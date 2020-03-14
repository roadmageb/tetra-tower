using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdBuffer : Addon, IAtkSkillUse
{
    public AdBuffer(ScriptableAddonInfo _info) : base(_info)
    {
        PlayerController.Instance.immuneVarDict.Add("AdBuffer", false);
    }

    public void AtkSkillUse(int skillNum)
    {
        PlayerController.Instance.immuneVarDict["AdBuffer"] = true;
        PlayerController.Instance.ExStartCoroutine(CancelImmune());
    }

    public IEnumerator CancelImmune()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerController.Instance.immuneVarDict["AdBuffer"] = false;
    }
}

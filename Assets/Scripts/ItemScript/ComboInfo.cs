using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboInfo
{
    public SkillInfo skill;
    [SerializeField] private string comboName;
    [SerializeField] private int[] comboAction;
    [SerializeField] private InputArrow comboArrow;
    [SerializeField] private PosCond positionCond;
    public float keyGain;
    public AnimationClip anim;
    public float[] damageList;
    private int comboSuccessCounter;

    public bool CheckCombo(InputArrow inputArrow, int inputAction, int globalSuccessCounter, out bool isComboEnd, out bool isPerfectCombo)
    {
        if(globalSuccessCounter == 0 || globalSuccessCounter == comboSuccessCounter)
        {
            if(comboAction[globalSuccessCounter] == inputAction)
            {
                if (globalSuccessCounter == comboAction.Length - 1 && (comboArrow == InputArrow.NULL || inputArrow == comboArrow))
                {
                    comboSuccessCounter = 0;
                    isComboEnd = true;
                    isPerfectCombo = inputArrow == comboArrow;
                    return true;
                }
                else if(globalSuccessCounter == comboAction.Length - 1)
                {
                    isComboEnd = false;
                    isPerfectCombo = false;
                    return false;
                }
                comboSuccessCounter = globalSuccessCounter + 1;
                isComboEnd = false;
                isPerfectCombo = false;
                return true;
            }
        }
        comboSuccessCounter = 0;
        isComboEnd = false;
        isPerfectCombo = false;
        return false;
    }
    
    public void DoCombo()
    {
        Debug.Log(comboName);
    }

    /// <summary>
    /// Check if this contains same command with chk
    /// </summary>
    /// <param name="chk"></param>
    /// <returns></returns>
    public bool CheckEqualCombo(ComboInfo chk)
    {
        if(comboAction.Length == chk.comboAction.Length && comboArrow == chk.comboArrow && positionCond == chk.positionCond)
        {
            for(int i = 0; i < comboAction.Length; i++)
            {
                if(comboAction[i] != chk.comboAction[i])
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}

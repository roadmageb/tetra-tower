using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboInfo
{
    public SkillInfo skill;
    [SerializeField] private int[] comboAction;
    [SerializeField] private InputArrow comboArrow;
    [SerializeField] private PosCond positionCond;
    [SerializeField] private float keyGain;
    public AnimationClip anim;
    public float[] damageList;
    private int comboSuccessCounter;

    //*******************Used for test****************************
    [SerializeField] private string comboName;
    public ComboInfo(string _comboName, int[] _comboAction, InputArrow _comboArrow, PosCond _positionCond)
    {
        comboName = _comboName;
        comboAction = _comboAction;
        comboArrow = _comboArrow;
        positionCond = _positionCond;
    }
    public void DoCombo()
    {
        Debug.Log(comboName);
    }
    //************************************************************


    /// <summary>
    /// Check combo
    /// </summary>
    /// <param name="inputArrow">Input arrow key currently inserted</param>
    /// <param name="inputAction">Input action key currently inserted</param>
    /// <param name="globalSuccessCounter">Combo success counter globally.</param>
    /// <param name="isComboEnd">Return value if this combo is ended</param>
    /// <param name="isPerfectCombo">Return value if this combo is perfect combo</param>
    /// <returns>Return if current combo is legal or not</returns>s
    public bool CheckCombo(InputArrow inputArrow, int inputAction, PosCond inputPosCond, int globalSuccessCounter, out bool isComboEnd, out bool isPerfectArrow, out bool isPerfectPosCond)
    {
        isComboEnd = isPerfectArrow = isPerfectPosCond = false;
        if (globalSuccessCounter == 0 || globalSuccessCounter == comboSuccessCounter)
        {
            if(comboAction[globalSuccessCounter] == inputAction)
            {
                if (globalSuccessCounter == comboAction.Length - 1 && (comboArrow == InputArrow.NULL || comboArrow == inputArrow) && (positionCond == PosCond.None || positionCond == inputPosCond))
                {
                    comboSuccessCounter = 0;
                    isComboEnd = true;
                    isPerfectArrow = comboArrow == inputArrow;
                    isPerfectPosCond = positionCond == inputPosCond;
                    return true;
                }
                else if(globalSuccessCounter == comboAction.Length - 1)
                {
                    return false;
                }
                comboSuccessCounter = globalSuccessCounter + 1;
                return true;
            }
        }
        comboSuccessCounter = 0;
        return false;
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

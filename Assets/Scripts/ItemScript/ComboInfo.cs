using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboInfo
{
    private int[] comboAction;
    private InputArrow comboArrow;
    private PosCond positionCond;
    private int comboSuccessCounter;
    private string comboName;
    private float keyGain;
    private int delayFrame;

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

    public ComboInfo(InputArrow _comboArrow, int[] _comboAction, float _keyGain, int _delayFrame, string _comboName)
    {
        comboArrow = _comboArrow;
        comboAction = _comboAction;
        comboName = _comboName;
        keyGain = _keyGain;
        delayFrame = _delayFrame;
        comboSuccessCounter = 0;
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

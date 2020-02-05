using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboInfo
{
    private int[] comboAction;
    private InputArrow comboArrow;
    private int comboSuccessCounter;
    private string comboName;

    public bool CheckCombo(InputArrow inputArrow, int inputAction, int globalSuccessCounter)
    {
        if(globalSuccessCounter == 0 || globalSuccessCounter == comboSuccessCounter)
        {
            if(comboAction[globalSuccessCounter] == inputAction)
            {
                if (globalSuccessCounter == comboAction.Length - 1 && inputArrow == comboArrow)
                {
                    Debug.Log(comboName);
                    comboSuccessCounter = 0;
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

    public ComboInfo(InputArrow _comboArrow, int[] _comboAction, string _comboName)
    {
        comboArrow = _comboArrow;
        comboAction = _comboAction;
        comboName = _comboName;
        comboSuccessCounter = 0;
    }
}

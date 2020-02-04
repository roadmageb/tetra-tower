using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboInfo
{
    public int[] comboAction;
    public InputArrow comboArrow;
    public int comboSuccessCounter, comboTimer = 40, comboCounter = 0;

    public void CheckCombo(InputArrow inputArrow, int inputAction)
    {
        Debug.Log(comboCounter);
        if (comboSuccessCounter == 0)
        {
            comboCounter = 0;
        }
        else
        {
            comboCounter++;
        }
        if (comboAction[comboSuccessCounter] == inputAction && comboCounter < comboTimer)
        {
            comboCounter = 0;
            comboSuccessCounter++;
            if (comboSuccessCounter == comboAction.Length)
            {
                if (comboArrow == inputArrow)
                {
                    //Do combo action
                    Debug.Log("Combo");
                }
                comboSuccessCounter = 0;
            }
        }
        else if (inputAction != 0 || comboCounter >= comboTimer)
        {
            comboSuccessCounter = 0;
        }
    }

    public ComboInfo(InputArrow _comboArrow, int[] _comboAction)
    {
        comboArrow = _comboArrow;
        comboAction = _comboAction;
        comboSuccessCounter = 0;
    }
}

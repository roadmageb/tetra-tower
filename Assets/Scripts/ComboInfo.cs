using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboInfo
{
    public int[] comboAction;
    public InputArrow comboArrow;
    public int comboSuccessCounter, comboTimer = 60, comboCounter = 0;

    public void CheckCombo(InputArrow inputArrow, int inputAction)
    {
        //Debug.Log(comboCounter);

        if (comboSuccessCounter < comboAction.Length - 1)
        {
            if (comboAction[comboSuccessCounter] == inputAction)
            {
                comboSuccessCounter++;
            }
            else
            {
                comboSuccessCounter = 0;
            }
        }
        else
        {
            if (comboAction[comboSuccessCounter] == inputAction && comboArrow == inputArrow)
            {
                Debug.Log("Combo");
            }
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

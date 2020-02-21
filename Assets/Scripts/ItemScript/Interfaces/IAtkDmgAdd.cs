using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAtkDmgAdd
{
    float AtkDmgAdd(AttackPtoE attack, int skillNum, Enemy enemy);
}

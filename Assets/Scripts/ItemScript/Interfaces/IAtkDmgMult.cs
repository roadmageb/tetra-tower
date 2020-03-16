using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAtkDmgMult
{
    float AtkDmgMult(AttackPtoE attack, int skillNum, Enemy enemy);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAtkOnHit
{
    void AtkOnHit(AttackPtoE attack, Transform attacker, int skillNum, Enemy enemy);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdFeatherOfPigeon : Addon, IAtkOnHit
{
    public AdFeatherOfPigeon(ScriptableAddonInfo _info) : base(_info)
    {
    }

    public void AtkOnHit(AttackPtoE attack, Transform attacker, int skillNum, Enemy enemy)
    {
        Transform tEnemy = enemy.transform;
    }
}

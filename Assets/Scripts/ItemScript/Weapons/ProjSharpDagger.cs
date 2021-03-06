﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjSharpDagger : PlayerAttackBase
{
    SkillInfo skill;
    List<Enemy> dupCheck;

    public void SetProjectile(SkillInfo skill, Vector3 pos, float scale)
    {
        this.skill = skill;
        transform.position = pos;
        transform.localScale = new Vector3(scale, 1, 1);
        gameObject.SetActive(true);
        dupCheck = new List<Enemy>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            Enemy enemy = collision.transform.GetComponent<Enemy>();
            if (!dupCheck.Contains(enemy))
            {
                dupCheck.Add(enemy);
                enemy.GainAttack(skill.wp.CalcAttack(skill.num, enemy));
            }
        }
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
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
        dupCheck = new List<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            Enemy enemy = collision.transform.GetComponent<Enemy>();
            if (!dupCheck.Contains(enemy))
            {
                dupCheck.Add(enemy);
                skill.wp.ExecuteAttack(skill.num, enemy);
            }
        }
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}

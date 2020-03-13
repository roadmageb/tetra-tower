using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjBow : PlayerAttackBase
{
    SkillInfo skill;

    public void SetProjectile(SkillInfo skill, Vector3 pos, bool isUp, int dir)
    {
        this.skill = skill;
        transform.position = pos;
        if(isUp)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 20);
        }
        else
        {
            transform.localScale = new Vector3(dir, 1, 1);
            GetComponent<Rigidbody2D>().velocity = new Vector2(dir * 20, 0);
        }
        gameObject.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            Enemy enemy = collision.transform.GetComponent<Enemy>();
            enemy.GainAttack(skill.wp.CalcAttack(skill.num, enemy));
            Destroy(gameObject);
        }
        else if(collision.transform.CompareTag("Floor"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(DelayDestroy());
        }
    }
    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}

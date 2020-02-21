using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[System.Serializable]
public class AttackPattern
{
    public AnimationClip attackAnim;
    public int attackDamage;
    public CtrlEtoP attackCtrl;
}

public abstract class Enemy : MonoBehaviour
{
    public Transform target;
    public Animator animator;
    private AnimatorOverrideController animOverCont;
    [SerializeField] protected float speed = 200;
    [SerializeField] protected float nextWayPointDistance = 3;
    [SerializeField] protected float playerDetectDistance = 1;
    [SerializeField] protected float attackRange = 0.5f;
    [SerializeField] protected Vector2 groundDetectOffset;
    [SerializeField] protected float attackDamage;
    [SerializeField] private AnimationClip idleAnim, traceAnim, damagedAnim, deathAnim;
    [SerializeField] public AttackPattern[] attackPattern;
    public bool attackFollowPlayer;
    public bool playerAttackable = false;
    public bool attackedPlayer = false;
    public int currentAttackIndex = 0;
    
    Path path;
    Seeker seeker;
    int currentWaypoint = 0;
    public float traceTime = 0;
    float traceTimeLimit = 3;
    protected bool seekTarget = false;
    protected Rigidbody2D rb;

    public float maxHP;
    public float currentHP;
    public EnemyCtrl enemyCtrl;

    public List<AttackMethod> attackMethods;
    public delegate void AttackMethod();

    public virtual void AttackStart()
    {
        currentAttackIndex = Random.Range(0, attackPattern.Length);
        animOverCont["EnemyAttackAnim"] = attackPattern[currentAttackIndex].attackAnim;
        attackedPlayer = false;
        animator.SetBool("Attack", true);
    }

    public virtual void AttackEnd()
    {
        attackedPlayer = false;
        animator.SetBool("Attack", false);
    }

    public void GainAttack(AttackPtoE attack)
    {
        GetDamage(attack.damage);
        enemyCtrl.ApplyCtrl(attack);
    }

    public void GetDamage(float damage)
    {
        currentHP -= damage * (enemyCtrl.isFreeze ? 1.5f : 1);
        if (enemyCtrl.isFreeze)
        {
            enemyCtrl.EndFreeze();
        }
        if (currentHP <= 0)
        {
            Death();
        }
        else
        {
            animator.SetTrigger("Damaged");
        }
    }

    public void Death()
    {
        animator.SetTrigger("Death");
        Debug.Log("I die");
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    /// <summary>
    /// Seek and trace target
    /// </summary>
    /// <returns>returns if seek ends</returns>
    public virtual void SeekTarget()
    {
        seekTarget = Physics2D.Raycast(rb.position, Vector3.right * (transform.localScale.x > 0 ? 1 : -1), playerDetectDistance, LayerMask.GetMask("Player"));
        if (seekTarget || Time.time - traceTime < traceTimeLimit)
        {
            UpdatePath();
            if (seekTarget)
            {
                traceTime = Time.time;
            }

            if (path == null)
            {
                animator.SetBool("Trace", false);
                return;
            }
            else
            {
                animator.SetBool("Trace", true);
            }

            if (Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                AttackStart();
            }

           Vector2 force = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized * speed;
            RaycastHit2D checkGround = Physics2D.Raycast((Vector2)transform.position + Vector2.right * groundDetectOffset * (target.position.x - transform.position.x > 0 ? 1 : -1),
                Vector2.down, GetComponent<Collider2D>().bounds.size.y, LayerMask.GetMask("Floor"));
            if (!checkGround.collider)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                rb.AddForce(force);
                transform.localScale = new Vector3(1 * (target.position.x - transform.position.x > 0 ? 1 : -1), 1, 1);
            }

            if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < nextWayPointDistance)
            {
                currentWaypoint++;
            }
        }
        else
        {
            animator.SetBool("Trace", false);
        }
    }

    protected void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    protected virtual void Start()
    {
        enemyCtrl = GetComponent<EnemyCtrl>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player").transform;
        traceTime = -traceTimeLimit;
        currentHP = maxHP;

        animOverCont = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animOverCont;
        animOverCont["EnemyIdleAnim"] = idleAnim;
        animOverCont["EnemyTraceAnim"] = traceAnim;
        animOverCont["EnemyDamagedAnim"] = damagedAnim;
        animOverCont["EnemyDeathAnim"] = deathAnim;
        attackMethods = new List<AttackMethod>();
        //InvokeRepeating("UpdatePath", 0, 0.1f);
    }

    private void Update()
    {
        if (Physics2D.Raycast(rb.position, Vector3.right * (transform.localScale.x > 0 ? 1 : -1), playerDetectDistance, LayerMask.GetMask("Player")))
        {
            animator.SetBool("Trace", true);
        }
    }
}

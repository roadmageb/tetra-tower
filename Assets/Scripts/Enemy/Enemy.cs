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
    [SerializeField] private AnimationClip idleAnim, traceAnim, damagedAnim, deathAnim;
    [SerializeField] public AttackPattern[] attackPattern;
    [SerializeField] private EnemyDetectType detectType;
    [SerializeField] private bool ignoreGround;
    [SerializeField] private float lastAttackedTime;
    [SerializeField] private float attackDelay;

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
        animator.SetTrigger(currentHP <= 0 ? "Death" : "Damaged");
    }

    public virtual void DeathAction()
    {
        Debug.Log("I die");
        Destroy(animator.gameObject);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerAttackable && !attackedPlayer && collision.gameObject.tag.Equals("Player"))
        {
            attackedPlayer = true;
            PlayerController.Instance.GetDamage(attackPattern[currentAttackIndex].attackDamage);
        }
    }

    private bool DetectPlayer()
    {
        switch (detectType)
        {
            case EnemyDetectType.Line:
                return Physics2D.Raycast(rb.position, Vector3.right * (transform.localScale.x > 0 ? 1 : -1), playerDetectDistance, LayerMask.GetMask("Player"));
            case EnemyDetectType.Circle:
                return Physics2D.OverlapCircle(rb.position, playerDetectDistance, LayerMask.GetMask("Player"));
            default:
                return false;
        }
    }

    public virtual void IdleAction() { }

    /// <summary>
    /// Seek and trace target
    /// </summary>
    /// <returns>returns if seek ends</returns>
    public virtual void TraceAction()
    {
        if (DetectPlayer() || Time.time - traceTime < traceTimeLimit)
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

            if (Time.time - lastAttackedTime > attackDelay && Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                AttackStart();
            }

            Vector2 force = ((Vector2)path.vectorPath[currentWaypoint + 1] - rb.position).normalized * speed;
            RaycastHit2D checkGround = Physics2D.Raycast((Vector2)transform.position + Vector2.right * groundDetectOffset * (target.position.x - transform.position.x > 0 ? 1 : -1),
                Vector2.down, GetComponent<Collider2D>().bounds.size.y, LayerMask.GetMask("Floor"));
            if (!ignoreGround && !checkGround.collider)
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
        lastAttackedTime = Time.time;
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

        lastAttackedTime = -attackDelay;
    }

    private void Update()
    {
        if (DetectPlayer())
        {
            animator.SetBool("Trace", true);
        }
    }
}

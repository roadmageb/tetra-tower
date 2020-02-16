using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class Enemy : MonoBehaviour
{
    public Transform target;
    public Animator animator;
    [SerializeField] protected float speed = 200;
    [SerializeField] protected float nextWayPointDistance = 3;
    [SerializeField] protected float playerDetectDistance = 1;
    [SerializeField] protected float attackRange = 0.5f;
    [SerializeField] protected Vector2 groundDetectOffset;
    public bool attackFollowPlayer = false;

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

    public virtual void Attack()
    {
        
    }

    public virtual void AttackEnd()
    {
        animator.SetTrigger("AttackEnd");
    }

    public void GainAttack(AttackPtoE attack)
    {
        currentHP -= attack.damage;
        if (currentHP <= 0)
        {
            Death();
        }
        enemyCtrl.ApplyCtrl(attack);
    }

    public void Death()
    {
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
                animator.SetTrigger("Attack");
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
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player").transform;
        traceTime = -traceTimeLimit;


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

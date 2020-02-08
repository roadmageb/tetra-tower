using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float speed = 200;
    [SerializeField] private float nextWayPointDistance = 3;
    [SerializeField] private float playerDetectDistance = 1;
    [SerializeField] private LayerMask playerLayer;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false, seekTarget = false;
    Seeker seeker;
    Rigidbody2D rb;

    public float maxHP;
    public float currentHP;
    public EnemyCtrl enemyCtrl;

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

    void SeekTarget()
    {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;

        rb.AddForce(force);
        if (force.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (force.x <= 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0, 0.1f);
    }

    private void Update()
    {
        seekTarget = Physics2D.OverlapCircle(rb.position, playerDetectDistance, playerLayer) != null;
    }

    private void LateUpdate()
    {
        if (seekTarget)
        {
            SeekTarget();
        }
    }
}

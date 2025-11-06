using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Target & Movement")]
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDist = 3f;
    public float detectionRange = 10f;
    public Transform birdGFX;
    public Animator animator;

    [Header("Rest Points")]
    public List<Transform> restPoints;

    [Header("Combat Settings")]
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask playerLayer;


    private float lastDamageTime;
    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Transform currentTarget;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);

    }

    void UpdatePath()
    {
        float distanceToTarget = Vector2.Distance(rb.position, target.position);

        if (distanceToTarget < detectionRange)
        {
            //Játékos üldözés
            currentTarget = target;
        }
        else
        {
            //Pihenő pont keresés
            currentTarget = GetClosestRestPoint();
        }

        if(currentTarget != null && seeker.IsDone())
        {
            seeker.StartPath(rb.position, currentTarget.position, OnPathComplete);
        }
    }

    Transform GetClosestRestPoint()
    {
        Transform closestPoint = null;
        float closestDist = Mathf.Infinity;

        foreach (Transform point in restPoints)
        {
            float distance = Vector2.Distance(rb.position, point.position);
            if (distance < closestDist)
            {
                closestDist = distance;
                closestPoint = point;
            }
        }
        return closestPoint;
    }

        void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void FixedUpdate()
    {
        if (path == null)
        {
            animator.SetBool("isMoving", false);
            return;
        }

        HandlePlayerContactDamage();

        float distanceToTarget = Vector2.Distance(rb.position, currentTarget.position);

        if(currentTarget != target && distanceToTarget < nextWaypointDist)
        {
            animator.SetBool("isMoving", false);
            rb.velocity = Vector2.zero;
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            animator.SetBool("isMoving", false);
            return;
        }


        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        animator.SetBool("isMoving", true);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDist)
        {
            currentWaypoint++;
        }

        //Flip sprite
        if (force.x >= 0.01f)
        {
            birdGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            birdGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void HandlePlayerContactDamage()
    {
        if (Time.time < lastDamageTime + damageCooldown)
            return;

        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (hitPlayer != null)
        {
            Damageable dmg = hitPlayer.GetComponent<Damageable>();
            if (dmg != null)
            {
                Vector2 knockbackDir = (hitPlayer.transform.position - transform.position).normalized;
                Vector2 knockback = knockbackDir * knockbackForce;

                bool hit = dmg.Hit(contactDamage, knockback);
                if (hit)
                {
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}

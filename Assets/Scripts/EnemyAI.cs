using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDist = 3f;
    public float detectionRange = 10f;

    public Transform birdGFX;
    public Animator animator;

    //Pihenő pontok
    public List<Transform> restPoints;

    Path path;
    int currentWaypoint = 0;

    Seeker seeker;
    Rigidbody2D rb;

    Transform currentTarget;

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
}

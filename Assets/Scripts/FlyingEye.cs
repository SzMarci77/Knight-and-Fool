using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public Collider2D deathCollidor;
    public List<Transform> waypoints;
    

    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

    Transform nextWaypoint;
    int waypointNum = 0;

    public bool _hasTarget = false;
    

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(Animations.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(Animations.canMove);
        }
    }    

       private void Awake()
        {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
        }

    private void Start()
    {
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError($"{name}: nincs beállítva waypoint!");
            enabled = false;
            return;
        }

        nextWaypoint = waypoints[waypointNum];
    }


    private void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Flight()
    {
        // Következő waypoint felé irány
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        // Elérte-e a waypointot
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();

        // Kellet-e váltani a következő waypointra
        if (distance <= waypointReachedDistance)
        {
            waypointNum++;

            if(waypointNum >= waypoints.Count)
            {
                // Vissza az elsőhöz
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        if (rb.velocity.x > 0 && transform.localScale.x < 0)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void OnDeath()
    {
        if (rb != null)
        {
            rb.gravityScale = 2f;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (deathCollidor != null)
        {
            deathCollidor.enabled = true;
        }
    }
}

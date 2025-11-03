using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDir), typeof(Damageable))]
public class KnightEnemy : MonoBehaviour
{
    [Header("Detection Zones")]
    [SerializeField] private DetectionZone attackZone;
    [SerializeField] private DetectionZone cliffDetectionZone;

    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 3f;
    [SerializeField] private float walkAccelaration = 30f;
    [SerializeField] private float walkStopRate = 0.06f;

    Rigidbody2D rb;
    TouchingDir touchingDir;
    Animator animator;
    Damageable damageable;

    private Vector3 originalScale;

    public enum WalkingDirection { Right, Left };
    public Vector2 walkDirectionVector = Vector2.right;
    private WalkingDirection _walkDirection;


    public WalkingDirection WalkDirection
    {
        get => _walkDirection;
        set
        {
            if (_walkDirection != value)
            {
                _walkDirection = value;
                // Flip az eredeti skála alapján
                transform.localScale = new Vector3(originalScale.x * (_walkDirection == WalkingDirection.Right ? 1 : -1),
                                                   originalScale.y,
                                                   originalScale.z);
                walkDirectionVector = (_walkDirection == WalkingDirection.Right) ? Vector2.right : Vector2.left;
            }
        }
    }

    public bool _hastarget = false;
    public bool HasTarget
    {
        get => _hastarget;
        private set
        {
            _hastarget = value;
            if (animator != null)
                animator.SetBool(Animations.hasTarget, value);
        }
    }

    public bool CanMove => animator != null && animator.GetBool(Animations.canMove);

    public float AttackCooldown
    {
        get => animator != null ? animator.GetFloat(Animations.attackCooldown) : 0;
        private set
        {
            if (animator != null)
                animator.SetFloat(Animations.attackCooldown, Mathf.Max(value, 0));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDir = GetComponent<TouchingDir>();
        animator = GetComponent<Animator>();
        animator.SetBool(Animations.canMove, true);
        damageable = GetComponent<Damageable>();
        originalScale = transform.localScale;

    }

    void Update()
    {
        // Ellenőrizzük, van-e támadható célpont
        HasTarget = attackZone.detectedColliders.Count > 0;

        if(AttackCooldown > 0 )
        {
            AttackCooldown -= Time.deltaTime;
        }
    }


    private void FixedUpdate()
    {
        if (touchingDir.IsGrounded && touchingDir.IsOnWall)
        {
            FlipDirection();
        }

        if (!damageable.LockVelocity)
        {
            if (CanMove)
            {
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (walkAccelaration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }


        
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkingDirection.Right)
        {
            WalkDirection = WalkingDirection.Left;
        }
        else if (WalkDirection == WalkingDirection.Left)
        {
            WalkDirection = WalkingDirection.Right;
        }
        else
        {
            Debug.LogError("Nincs helyes érték megadva: left/right");
        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {
      rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDir.IsGrounded)
        {
            FlipDirection();
        }
    }
}

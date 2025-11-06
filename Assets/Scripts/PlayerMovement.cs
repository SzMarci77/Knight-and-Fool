using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDir), typeof(Damageable))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float airWalkSpeed = 3f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 20f;

    [Header("Jump")]
    [SerializeField] private float jumpStrength = 10f;
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    private Rigidbody2D rb;
    private Animator animator;
    private TouchingDir touchingDirections;
    private Damageable damageable;
    private TrailRenderer trail;

    private Vector2 moveInput;
    private float currentTargetSpeed = 0f;
    private float coyoteTimer;
    private float jumpBufferTimer = 0f;
    private bool isJumping = false;
    private bool _isFacingRight = true;
    private Vector3 originalScale;

    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isRunning = false;
    public bool CanMove => animator != null && animator.GetBool(Animations.canMove);
    public bool IsAlive => animator != null && animator.GetBool(Animations.isAlive);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDir>();
        damageable = GetComponent<Damageable>();
        originalScale = transform.localScale;
        trail = GetComponent<TrailRenderer>();

        if (trail != null)
            trail.emitting = false;
    }

    private void FixedUpdate()
    {

        if (!damageable.LockVelocity)
        {
            float targetSpeed;

            if (Mathf.Abs(moveInput.x) > 0.01f)
            {
                targetSpeed = moveInput.x * (touchingDirections.IsGrounded ? CurrentMoveSpeed : runSpeed);
            }
            else
            {
                targetSpeed = 0f;
            }

            float currentSpeed = rb.velocity.x;

            float accelRate = (Mathf.Abs(moveInput.x) > 0.01f) ? acceleration : deceleration;
            if (!touchingDirections.IsGrounded)
                accelRate *= 0.5f;

            currentTargetSpeed = Mathf.MoveTowards(currentTargetSpeed, targetSpeed, accelRate * Time.fixedDeltaTime);

            rb.velocity = new Vector2(currentTargetSpeed, rb.velocity.y);
        }

        if (animator != null)
            animator.SetFloat(Animations.yVelocity, rb.velocity.y);

        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.fixedDeltaTime;
        }

        if (jumpBufferTimer > 0f && coyoteTimer > 0f && CanMove && !isJumping)
        {
            if (animator != null)
                animator.SetTrigger(Animations.jumpTrigger);

            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);

            if (trail != null)
                trail.emitting = true;

            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
            isJumping = true;
        }

        if (touchingDirections.IsGrounded && !isJumping)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }


    }

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                //Földön
                if (touchingDirections.IsGrounded)
                {
                    return IsRunning ? runSpeed : walkSpeed;
                }
                //Falon nincs mozgás
                else if (touchingDirections.IsOnWall)
                {
                    return 0f;
                }
                //Levegőben
                else
                {
                    return airWalkSpeed;
                }
            }
            else
            {
                //Nincs mozgás
                return 0f;
            }
            
        }
    }

    public bool IsMoving
    {
        get => _isMoving;
        private set 
        {
            _isMoving = value;
            if (animator != null)
                animator.SetBool(Animations.isMoving, value);
        } 
    }

    public bool IsRunning
    {
        get => _isRunning;
        private set
        {
            _isRunning = value;
            if (animator != null)
                animator.SetBool(Animations.isRunning, value);
        }
    }

    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            if (_isFacingRight != value)
            {
                _isFacingRight = value;
                // Flip a karakter irányához az eredeti skála alapján
                transform.localScale = new Vector3(originalScale.x * (_isFacingRight ? 1 : -1),
                                                   originalScale.y,
                                                   originalScale.z);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (PauseMenu.GameIsPaused)
            return;

        moveInput = context.ReadValue<Vector2>();


        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }

    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0) IsFacingRight = true;
        else if (moveInput.x < 0) IsFacingRight = false;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (PauseMenu.GameIsPaused)
            return;

        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (PauseMenu.GameIsPaused)
            return;

        if (context.started && animator != null)
            animator.SetTrigger(Animations.attackTrigger);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started && coyoteTimer > 0f && CanMove)
        {
            if (animator != null)
                animator.SetTrigger(Animations.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
            if (trail != null)
                trail.emitting = true;

            coyoteTimer = 0f;
            isJumping = true;

            jumpBufferTimer = jumpBufferTime;
        }
    }


    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    private bool wasGrounded = false;
    private void Update()
    {
        bool isGrounded = touchingDirections.IsGrounded;

        if(!wasGrounded && isGrounded)
        {
            if (trail != null) trail.emitting = false;
            isJumping = false;
        }
        wasGrounded = isGrounded;
    }
}

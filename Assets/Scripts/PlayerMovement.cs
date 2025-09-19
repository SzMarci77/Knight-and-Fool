using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDir), typeof(Damageable))]

public class PlayerMovement : MonoBehaviour
{
    public float jumpStrength = 10f;
    public float walkSpeed = 5f;
    public float airWalkSpeed = 3f;
    public float runSpeed = 8f;

    private Vector3 originalScale;

    Vector2 moveInput;

    Rigidbody2D rb;
    Animator animator;
    TouchingDir touchingDirections;
    Damageable damageable;

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


    [SerializeField] private bool _isMoving = false;
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

    [SerializeField] private bool _isRunning = false;

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

    private bool _isFacingRight = true;
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



    public bool CanMove => animator != null && animator.GetBool(Animations.canMove);
    public bool IsAlive => animator != null && animator.GetBool(Animations.isAlive);


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDir>();
        damageable = GetComponent<Damageable>();
        originalScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        if(!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        if (animator != null)
            animator.SetFloat(Animations.yVelocity, rb.velocity.y);
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
        //Földön van-e a játékos + életben
        if(context.started && touchingDirections.IsGrounded && CanMove)
        {
            if (animator != null)
                animator.SetTrigger(Animations.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}

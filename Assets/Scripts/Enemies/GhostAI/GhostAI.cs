using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour, IDamageable
{
    #region Variables
    public EnemyBaseState currentState;

    public PatrolState patrolState;
    public PlayerDetectedState playerDetectedState;
    public ChargeState chargeState;
    public MeleeAttackState meleeAttackState;
    public DamagedState damagedState;
    public DeathState deathState;
    public DodgeState dodgeState;

    public Animator anim;
    public Rigidbody2D rb;
    public Transform ledgeDetector;
    public LayerMask groundLayer, playerLayer;

    public int facingDirection = 1;

    public float stateTime;

    public StatsSO stats;
    public float currentHealth;


    public GameObject alert;

    [Header("Death Particle Drop")]
    public float dropForce;
    public float torque;

    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        patrolState = new PatrolState(this, "patrol");
        playerDetectedState = new PlayerDetectedState(this, "playerDetected");
        chargeState = new ChargeState(this, "charge");
        meleeAttackState = new MeleeAttackState(this, "meleeAttack");
        damagedState = new DamagedState(this, "damaged");
        deathState = new DeathState(this, "death");
        dodgeState = new DodgeState(this, "dodge");

        currentState = patrolState;
        currentState.Enter();
    }

    private void Start()
    {
        currentHealth = stats.maxHealth;

    }


    private void Update()
    {
        currentState.LogicUpdate();
    }

    public void FixedUpdate()
    {
        currentState.PhysicsUpdate();

    }
    #endregion

    #region Checks
    public bool CheckForObstacles()
    {
        RaycastHit2D hit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, stats.raycastDistance, groundLayer);

        if (hit.collider == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckForPlayer()
    {
        RaycastHit2D hitPlayer = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1 ? Vector2.right : Vector2.left, stats.playerDetectDistance, playerLayer);
        if (hitPlayer.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckIfShouldDodge()
    {
        RaycastHit2D hitPlayer = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1 ? Vector2.right : Vector2.left, stats.dodgeDetectDistance, playerLayer);
        bool aggressivePlayer = facingDirection > 0 && Input.GetAxis("Horizontal") < 0 || facingDirection < 0 && Input.GetAxis("Horizontal") > 0;
        if (hitPlayer && aggressivePlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckForMeleeTarget()
    {
        RaycastHit2D hitMeleeTarget = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1 ? Vector2.right : Vector2.left, stats.meleeAttackDistance, playerLayer);
        if (hitMeleeTarget.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Other Functions
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(ledgeDetector.position, (facingDirection == 1 ? Vector2.right : Vector2.left) * 5);
    }


    public void SwitchState(EnemyBaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
        stateTime = Time.time;
    }

    public void AnimationFinishedTrigger()
    {
    currentState.AnimationFinishedTrigger();
    }

    public void AnimationAttackTrigger()
    {
    currentState.AnimationAttackTrigger();
    }

    public void Damagee(int damageAmount, Vector2 knockbackAngle) { }

    public void Damagee(int damageAmount, float KBForce, Vector2 knockbackAngle)
    {
        damagedState.KBForce = KBForce;
        damagedState.KBAngle = knockbackAngle;
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            SwitchState(deathState);
        }
        else
        {
            SwitchState(damagedState);
        }
    }

    public void Rotate()
    {
        transform.Rotate(0, 180, 0);
        facingDirection = -facingDirection;
    }

    public void Instantiate(GameObject prefab, float dropForce, float torque)
    {
        Rigidbody2D particleRb = Instantiate(prefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        Vector2 dropVelocity = new Vector2(Random.Range(0.5f, -0.5f), 1) * dropForce;
        particleRb.AddForce(dropVelocity, ForceMode2D.Impulse);
        particleRb.AddTorque(torque, ForceMode2D.Impulse);
    }
    #endregion
}

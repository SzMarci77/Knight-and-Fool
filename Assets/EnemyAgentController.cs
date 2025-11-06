using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAgentController : Agent
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask; // legyen rajta a fal layer is
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 0.6f;
    [SerializeField] private float knockbackForce = 5f;

    private bool _isFacingRight = false;
    public bool IsFacingRight
    {
        get => _isFacingRight;
        set
        {
            if (_isFacingRight != value)
            {
                _isFacingRight = value;
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * (_isFacingRight ? -1f : 1f);
                transform.localScale = scale;
            }
        }
    }

    private float lastAttackTime;
    private float jumpCD;
    private float prevDistance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundMask);
    }

    public override void OnEpisodeBegin()
    {
        prevDistance = Mathf.Abs(playerTransform.position.x - transform.position.x);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Saját pozíció
        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.y);

        // Játékos pozíció
        sensor.AddObservation(playerTransform.position.x);
        sensor.AddObservation(playerTransform.position.y);

        // Sebesség
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);

        // Talajon van-e
        sensor.AddObservation(IsGrounded() ? 1f : 0f);

        // Falérzékelés (két raycast)
        float lookAheadDistance = 2f;
        Vector2 direction = IsFacingRight ? Vector2.right : Vector2.left;

        Vector2 originMid = (Vector2)transform.position + Vector2.up * 0.5f;
        Vector2 originLow = (Vector2)transform.position + Vector2.down * 0.3f;

        RaycastHit2D hitMid = Physics2D.Raycast(originMid, direction, lookAheadDistance, groundMask);
        RaycastHit2D hitLow = Physics2D.Raycast(originLow, direction, lookAheadDistance, groundMask);

        bool wallDetected = hitMid.collider != null || hitLow.collider != null;

        sensor.AddObservation(wallDetected ? 1f : 0f);
        sensor.AddObservation(hitMid.collider ? hitMid.distance / lookAheadDistance : 1f);

        Debug.DrawRay(originMid, direction * lookAheadDistance, Color.red);
        Debug.DrawRay(originLow, direction * lookAheadDistance, Color.magenta);

        // Lefelé néző ray (platform szél)
        Vector2 downOrigin = (Vector2)transform.position + new Vector2(direction.x * 0.6f, 0);
        float downCheckDistance = 1.5f;
        RaycastHit2D groundAhead = Physics2D.Raycast(downOrigin, Vector2.down, downCheckDistance, groundMask);
        sensor.AddObservation(groundAhead.collider ? 1f : 0f);

        Debug.DrawRay(downOrigin, Vector2.down * downCheckDistance, Color.yellow);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int move = actions.DiscreteActions[0];   // 0=bal, 1=áll, 2=jobb
        int jump = actions.DiscreteActions[1];   // 0=nem, 1=igen
        int attack = actions.DiscreteActions[2]; // 0=nem, 1=igen

        float dir = 0f;
        if (move == 0) dir = -1f;
        else if (move == 2) dir = 1f;

        rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);

        if (dir > 0.01f)
            IsFacingRight = true;
        else if (dir < -0.01f)
            IsFacingRight = false;

        // Reward: ha közelebb kerül a játékoshoz
        float newDistance = Mathf.Abs(playerTransform.position.x - transform.position.x);
        if (newDistance < prevDistance) AddReward(0.01f);
        else AddReward(-0.005f);
        prevDistance = newDistance;

        // Büntetés: ha beragad
        if (Mathf.Abs(rb.velocity.x) < 0.1f && IsGrounded())
            AddReward(-0.002f);

        if (jump == 1 && Time.time - jumpCD > 1.5f)
        {
            jumpCD = Time.time;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AddReward(-0.01f); // kis negatív reward a felesleges ugrások elkerülésére
        }

        // Támadás
        if (attack == 1)
            DoAttack();

        // Leesés esetén
        if (transform.position.y < -5f)
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    private void DoAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        Vector2 dir = (playerTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, attackRange, playerLayer);

        if (hit.collider != null)
        {
            Damageable dmg = hit.collider.GetComponent<Damageable>();
            if (dmg != null && dmg.IsAlive)
            {
                Vector2 knockDir = (hit.collider.transform.position - transform.position).normalized;
                Vector2 knockback = knockDir * knockbackForce;
                bool success = dmg.Hit(attackDamage, knockback);
                if (success)
                {
                    AddReward(0.5f);
                    Debug.Log($"{name} attacked {hit.collider.name} for {attackDamage} dmg.");
                }
            }
        }

        Debug.DrawRay(transform.position, dir * attackRange, Color.red, 0.2f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var da = actionsOut.DiscreteActions;
        float h = Input.GetAxisRaw("Horizontal");
        da[0] = h < 0 ? 0 : h > 0 ? 2 : 1;
        da[1] = Input.GetKey(KeyCode.Space) ? 1 : 0;
        da[2] = Input.GetMouseButton(0) ? 1 : 0;
    }
}

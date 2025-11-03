using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAgentController : Agent
{
    private Rigidbody2D rb;
    public LayerMask groundMask;
    public Transform groundCheck;

    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundMask);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Csak alap szenzoradatok (a Behavior Cloning tanulásnál bővíthető)
        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.y);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);
        sensor.AddObservation(IsGrounded() ? 1f : 0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int move = actions.DiscreteActions[0];  // 0=bal, 1=áll, 2=jobb
        int jump = actions.DiscreteActions[1];  // 0=nem, 1=igen
        int attack = actions.DiscreteActions[2]; // most még nem kell

        float dir = 0f;
        if (move == 0) dir = -1f;
        else if (move == 2) dir = 1f;

        rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);

        if (jump == 1 && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var da = actionsOut.DiscreteActions;

        float h = Input.GetAxisRaw("Horizontal");
        if (h < 0) da[0] = 0;
        else if (h > 0) da[0] = 2;
        else da[0] = 1;

        da[1] = Input.GetKey(KeyCode.Space) ? 1 : 0;
        da[2] = Input.GetMouseButton(0) ? 1 : 0;
    }
}
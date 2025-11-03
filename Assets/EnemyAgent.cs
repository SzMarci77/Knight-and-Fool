using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EnemyAgent : Agent
{
    [Header("References")]
    public Rigidbody2D rb;
    public LayerMask groundMask;
    public Transform groundCheck;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    private bool isGrounded => Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundMask);

    // Mit "lát" az AI – ez majd később fontos a tanuláshoz
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.y);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);
        sensor.AddObservation(isGrounded ? 1f : 0f);
    }

    // Mit csinál a kapott akciók alapján
    public override void OnActionReceived(ActionBuffers actions)
    {
        int move = actions.DiscreteActions[0];  // 0=bal, 1=áll, 2=jobb
        int jump = actions.DiscreteActions[1];  // 0=nincs, 1=ugrás
        int attack = actions.DiscreteActions[2]; // 0=nincs, 1=támadás

        float dir = 0f;
        if (move == 0) dir = -1f;
        else if (move == 2) dir = 1f;

        rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);

        if (jump == 1 && isGrounded)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (attack == 1)
            Debug.Log("Attack!"); // ide jöhet később az animáció vagy sebzés
    }

    // Heuristic – itt te irányítod az Agentet billentyűzettel
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;

        // Mozgás
        float h = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
        if (h < 0) discreteActions[0] = 0;       // bal
        else if (h > 0) discreteActions[0] = 2;  // jobb
        else discreteActions[0] = 1;             // áll

        // Ugrás
        discreteActions[1] = Input.GetKey(KeyCode.Space) ? 1 : 0;

        // Támadás
        discreteActions[2] = Input.GetMouseButton(0) ? 1 : 0;
    }
}

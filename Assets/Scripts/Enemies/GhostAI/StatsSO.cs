using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StatsSO")]

public class StatsSO : ScriptableObject
{
    [Header("General")]
    public int maxHealth;

    [Header("Prefabs")]
    public GameObject deathParticle;

    [Header("Patrol State")]
    public float speed;
    public float raycastDistance;

    [Header("Player Detection")]
    public float playerDetectDistance;
    public float detectionPauseTime;
    public float playerDetectedWaitTime;

    [Header("Charge State")]
    public float chargeTime;
    public float chargeSpeed;
    public float meleeAttackDistance;

    [Header("Melee Attack State")]
    public int damageAmount;
    public Vector2 knockbackAngle;
    public float knockbackForce;

    [Header("Dodge State")]
    public Vector2 dodgeAngle;
    public float dodgeForce;
    public float dodgeDetectDistance;
    public float dodgeCooldown;
}

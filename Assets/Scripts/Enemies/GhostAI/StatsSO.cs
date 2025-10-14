using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StatsSO")]

public class StatsSO : ScriptableObject
{
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
}

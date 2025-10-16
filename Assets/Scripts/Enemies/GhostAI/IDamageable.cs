using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damagee(int damageAmount, Vector2 knockbackAngle);

    void Damagee(int damageAmount, float KBForce, Vector2 knockbackAngle);
}

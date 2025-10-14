using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damagee(int damageAmount, Vector2 knockbackAngle);
}

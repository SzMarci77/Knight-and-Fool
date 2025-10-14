using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : EnemyBaseState
{
    public MeleeAttackState(GhostAI enemy, string animationName) : base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemy.ledgeDetector.position, enemy.stats.meleeAttackDistance, enemy.playerLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();
            Rigidbody2D rb = hitCollider.GetComponent<Rigidbody2D>();

            if (damageable != null && rb != null)
            {
                // Számoljuk ki az ellenfél és a játékos közti irányvektort
                Vector2 knockbackDir = (hitCollider.transform.position - enemy.transform.position).normalized;

                // Sebzés
                damageable.Damagee(enemy.stats.damageAmount, knockbackDir * enemy.stats.knockbackForce);

                // Közvetlenül a Rigidbody2D-re is alkalmazzuk, a Hit után
                rb.velocity = new Vector2(
                    knockbackDir.x * enemy.stats.knockbackForce,
                    enemy.stats.knockbackAngle.y * enemy.stats.knockbackForce
                );
            }
        }

        // Támadás után visszaállás patrol state-re
        enemy.SwitchState(enemy.patrolState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

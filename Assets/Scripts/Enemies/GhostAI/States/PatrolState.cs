using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrolState : EnemyBaseState
{
    public PatrolState(GhostAI enemy, string animationName) : base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.CheckIfShouldDodge())
        {
            enemy.SwitchState(enemy.dodgeState);
        }
        else if (enemy.CheckForPlayer())
        {
            enemy.SwitchState(enemy.playerDetectedState);
        }
        else if (enemy.CheckForObstacles())
        {
            enemy.Rotate();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemy.facingDirection == 1)
        {
            enemy.rb.velocity = new Vector2(enemy.stats.speed, enemy.rb.velocity.y);
        }
        else
        {
           enemy.rb.velocity = new Vector2(-enemy.stats.speed, enemy.rb.velocity.y);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : EnemyBaseState
{
    public float KBForce;
    public Vector2 KBAngle;

    private float stunTime = 1;
    private bool isStunned;
    public DamagedState(GhostAI enemy, string animationName) : base(enemy, animationName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        ApplyKnockback();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isStunned)
        {
            if(Time.time > enemy.stateTime + stunTime)
            {
                isStunned = false;

                int forceDirection = enemy.rb.velocity.x > 0 ? -1 : 1;

                if(enemy.facingDirection != forceDirection)
                {
                    enemy.Rotate();
                }

                enemy.SwitchState(enemy.chargeState);

            }
        }
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        
        enemy.rb.velocity = new Vector2(enemy.rb.velocity.x, -enemy.rb.velocity.y);
        isStunned = true;
    }

    private void ApplyKnockback()
    {
    enemy.rb.velocity = KBAngle * KBForce;
    }
}

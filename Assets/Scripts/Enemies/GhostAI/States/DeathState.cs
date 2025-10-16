using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : EnemyBaseState
{
    public DeathState(GhostAI enemy, string animationName) : base(enemy, animationName)
    {
    }


    public override void Enter()
    {
        base.Enter();

        enemy.Instantiate(enemy.stats.deathParticle, enemy.dropForce, enemy.torque);

        enemy.gameObject.SetActive(false);

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


    public override void AnimationAttackTrigger()
    {
        base.AnimationAttackTrigger();
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
    }
}

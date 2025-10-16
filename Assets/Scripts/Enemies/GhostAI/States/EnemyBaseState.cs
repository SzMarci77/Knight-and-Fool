using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState
{
    protected GhostAI enemy;
    protected string animationName;

    public EnemyBaseState(GhostAI enemy, string animationName)
    {
        this.enemy = enemy;
        this.animationName = animationName;
    }

    public virtual void Enter()
    {
        enemy.anim.SetBool(animationName, true);
    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(animationName, false);

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void AnimationFinishedTrigger()
    {
    }

    public virtual void AnimationAttackTrigger()
    {
    }

}

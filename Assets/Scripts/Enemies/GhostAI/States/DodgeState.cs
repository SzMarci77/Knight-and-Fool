using UnityEngine;

public class DodgeState : EnemyBaseState
{
    private float endDodgeForce = 5f;
    private float endDodgeTime = 0.2f;
    private bool dodgeStarted;
    private bool dodgeEnded;
    public DodgeState(GhostAI ghostAI, string animBoolName) : base(ghostAI, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        enemy.rb.velocity = Vector2.zero;
        dodgeStarted = false;
        dodgeEnded = false;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time> enemy.stateTime + 0.05f && !dodgeStarted)
        {
        StartDodge();
        }

        if(Time.time >= enemy.stateTime + enemy.stats.dodgeCooldown)
        {
            if (enemy.CheckForPlayer())
            {
                enemy.SwitchState(enemy.playerDetectedState);
            }
            else
            {
                enemy.SwitchState(enemy.patrolState);
            }      
        }

        if(Time.time > enemy.stateTime + endDodgeTime && !dodgeEnded)
        {
            EndDodge();
        }
    }

    private void StartDodge()
    {
        enemy.rb.velocity = new Vector2(enemy.stats.dodgeAngle.x * -enemy.facingDirection, enemy.stats.dodgeAngle.y) * enemy.stats.dodgeForce;
        dodgeStarted = true;
    }

    private void EndDodge()
    {
    enemy.rb.velocity = new Vector2(0.1f * -enemy.facingDirection, -1) * endDodgeForce;
    dodgeEnded = true;
    }
}

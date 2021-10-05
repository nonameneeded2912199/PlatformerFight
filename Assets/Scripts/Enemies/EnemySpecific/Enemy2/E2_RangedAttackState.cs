using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_RangedAttackState : RangedAttackState
{
    private Enemy2 enemy;

    public E2_RangedAttackState(FiniteStateMachine stateMachine, Enemy2 enemy, string animBoolName, Transform attackPosition, D_RangedAttackState stateData) 
        : base(stateMachine, enemy, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();

        if (isAnimationFinished)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        float direction = entity.facingDirection == 1 ? 0 : Mathf.PI;

        for (float i = 0; i < 2 * Mathf.PI; i += (2 * Mathf.PI / 36))
        {
            /*GameObject projectile = Bullet.GetBullet(attackPosition.position, stateData.projectileSpeed, i,
            stateData.projectileLifeSpan, stateData.projectileDamage);*/
        }
    }
}

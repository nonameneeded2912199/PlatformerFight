using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_MeleeAttack : MeleeAttackState
{
    private Stage1Boss boss;

    public Stage1Boss_MeleeAttack(FiniteStateMachine stateMachine, Stage1Boss boss, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData)
        : base(stateMachine, boss, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(boss.incomingSpecialAttack);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.playerLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.transform.SendMessage("TakeDamage", attackDetails);
        }
    }
}

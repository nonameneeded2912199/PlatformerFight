using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_Phase1Melee : MeleeAttackState
{
    private Stage1Boss enemy;
    private Stage1Boss_Phase1 phase;

    private int attackNum = 0;

    public Stage1Boss_Phase1Melee(FiniteStateMachine stateMachine, Stage1Boss enemy, string animBoolName, 
        Transform attackPosition, D_MeleeAttackState stateData, Stage1Boss_Phase1 phase)
        : base(stateMachine, enemy, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
        this.phase = phase;
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
            if (enemy.CheckPlayerInCloseRangeAction())
            {
                isAnimationFinished = false;
                attackNum++;
                if (attackNum > 2)
                    attackNum = 0;
                enemy.CharacterAnimation.PlayAnim("Boss1_Melee" + attackNum);
            }
            else
            {
                phase.EnterMeleeCooldown();
                stateMachine.ChangeState(phase.ChasingPlayer);
            }
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

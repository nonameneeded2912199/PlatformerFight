using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_InitialState : IdleState
{
    private Stage1Boss boss;
    public Stage1Boss_InitialState(FiniteStateMachine stateMachine, Stage1Boss boss, string animBoolName, D_IdleState stateData)
        : base(stateMachine, boss, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();
        boss.SetInvincibility(true);
    }

    public override void Exit()
    {
        base.Exit();
        boss.SetInvincibility(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isIdleTimeOver)
        {
            if (playerInMinAgroRangeCheck)
            {
                boss.incomingSpecialAttack = boss.specialAttack1;
                stateMachine.ChangeState(boss.meleeAttack);
            }
            else
            {
                stateMachine.ChangeState(boss.specialAttack1);
            }                
        }    
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

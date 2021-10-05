using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_IdleState : IdleState
{
    private Enemy2 enemy;

    public E2_IdleState(FiniteStateMachine stateMachine, Enemy2 enemy, string animBoolName, D_IdleState stateData) 
        : base(stateMachine, enemy, animBoolName, stateData)
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (playerInMinAgroRangeCheck)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }

        if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

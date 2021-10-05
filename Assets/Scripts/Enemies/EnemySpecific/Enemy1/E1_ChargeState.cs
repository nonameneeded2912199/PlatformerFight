using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_ChargeState : ChargeState
{
    private Enemy1 enemy;
    public E1_ChargeState(FiniteStateMachine stateMachine, Enemy1 enemy, string animBoolName, D_ChargeState stateData) 
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

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }

        else if (wallCheck || !ledgeCheck)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }

        else if (isChargeTimeOver)
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

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

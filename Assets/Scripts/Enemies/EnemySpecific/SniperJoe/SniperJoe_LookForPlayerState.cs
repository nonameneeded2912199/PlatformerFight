using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperJoe_LookForPlayerState : LookForPlayerState
{
    private SniperJoe enemy;
    public SniperJoe_LookForPlayerState(FiniteStateMachine stateMachine, SniperJoe enemy, string animBoolName, D_LookForPlayerState stateData)
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
        //base.LogicUpdate();

        if (Time.time >= lastTurnTime + stateData.timeBetweenTurns)
        {
            entity.Flip();
            lastTurnTime = Time.time;
        }


        if (isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

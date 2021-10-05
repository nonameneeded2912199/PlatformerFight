using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingShooter_DeadState : DeadState
{
    private FlyingShooter enemy;

    public FlyingShooter_DeadState(FiniteStateMachine stateMachine, FlyingShooter enemy, string animBoolName, D_DeadState stateData)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

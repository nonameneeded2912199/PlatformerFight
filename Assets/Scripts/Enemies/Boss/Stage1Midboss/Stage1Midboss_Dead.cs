using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Midboss_Dead : DeadState
{
    private Stage1Midboss boss;
    public Stage1Midboss_Dead(FiniteStateMachine stateMachine, Stage1Midboss boss, string animBoolName, D_DeadState stateData)
        : base(stateMachine, boss, animBoolName, stateData)
    {
        this.boss = boss;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_Dead : DeadState
{
    private Stage1Boss boss;
    public Stage1Boss_Dead(FiniteStateMachine stateMachine, Stage1Boss boss, string animBoolName, D_DeadState stateData)
        : base(stateMachine, boss, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        boss.Rigidbody.gravityScale = 3f;
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

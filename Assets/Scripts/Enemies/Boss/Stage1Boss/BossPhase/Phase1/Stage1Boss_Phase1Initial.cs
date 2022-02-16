using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_Phase1Initial : IdleState
{
    private Stage1Boss boss;

    private Stage1Boss_Phase1 thisPhase;

    private bool moved = false;

    public Stage1Boss_Phase1Initial(FiniteStateMachine stateMachine, Stage1Boss boss, string animBoolName, D_IdleState stateData,
        Stage1Boss_Phase1 phase)
        : base(stateMachine, boss, animBoolName, stateData)
    {
        this.boss = boss;
        this.thisPhase = phase;
    }

    public override void Enter()
    {
        base.Enter();
        moved = false;
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

        if (isIdleTimeOver && !moved)
        {
            moved = true;
            boss.stateMachine.ChangeState(thisPhase.ChasingPlayer);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

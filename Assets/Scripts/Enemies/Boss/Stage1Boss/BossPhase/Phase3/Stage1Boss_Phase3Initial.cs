using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stage1Boss_Phase3Initial : IdleState
{
    private Stage1Boss boss;

    private Stage1Boss_Phase3 thisPhase;

    private bool moved = false;

    public Stage1Boss_Phase3Initial(FiniteStateMachine stateMachine, Stage1Boss boss, string animBoolName, D_IdleState stateData,
        Stage1Boss_Phase3 phase)
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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isIdleTimeOver && !moved)
        {
            moved = true;

            boss.Rigidbody.gravityScale = 0;

            boss.transform.DOMove(new Vector3(boss.transform.position.x, boss.highCenter.position.y), 0.5f).OnComplete(() =>
            {
                boss.stateMachine.ChangeState(thisPhase.BarrageState);
            });
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_Phase2Initial : IdleState
{
    private Stage1Boss boss;

    private Stage1Boss_Phase2 thisPhase;

    private bool moved = false;

    public Stage1Boss_Phase2Initial(FiniteStateMachine stateMachine, Stage1Boss boss, string animBoolName, D_IdleState stateData,
        Stage1Boss_Phase2 phase)
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

            Vector3 target = new Vector3(Random.Range(boss.leftMost.position.x, boss.rightMost.position.x), 
                boss.middleCenter.position.y, 0);

            thisPhase.MovingState.SetTargetJump(target);

            boss.stateMachine.ChangeState(thisPhase.MovingState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

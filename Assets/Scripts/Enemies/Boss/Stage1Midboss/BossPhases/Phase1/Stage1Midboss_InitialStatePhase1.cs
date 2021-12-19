using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stage1Midboss_InitialStatePhase1 : IdleState
{
    private Stage1Midboss boss;

    private Stage1Midboss_Phase1 thisPhase;

    private bool moved = false;

    public Stage1Midboss_InitialStatePhase1(FiniteStateMachine stateMachine, Stage1Midboss boss, string animBoolName, D_IdleState stateData,
        Stage1Midboss_Phase1 phase)
        : base(stateMachine, boss, animBoolName, stateData)
    {
        this.boss = boss;
        this.thisPhase = phase;
    }

    public override void Enter()
    {
        base.Enter();
        boss.SetInvincibility(true);
        boss.Rigidbody.gravityScale = 0;
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
            Debug.Log("here");
            Tween tween = boss.transform.DOMoveY(boss.flightLevel1.position.y, 0.5f).OnComplete(
                () =>
                {
                    boss.stateMachine.ChangeState(thisPhase.teleportOutState);
                    //boss.SetInvincibility(false);
                }
                );
        }    
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Midboss_InitialStatePhase2 : IdleState
{
    private Stage1Midboss boss;

    private Stage1Midboss_Phase2 phase;

    public Stage1Midboss_InitialStatePhase2(FiniteStateMachine stateMachine, Stage1Midboss entity, string animBoolName, D_IdleState stateData, Stage1Midboss_Phase2 phase) 
        : base(stateMachine, entity, animBoolName, stateData)
    {
        this.boss = entity;
        this.phase = phase;
    }

    public override void DoChecks()
    {
        base.DoChecks();
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
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        if (isIdleTimeOver)
        {
            boss.SetInvincibility(false);
            stateMachine.ChangeState(phase.moveState);
        }    
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

using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Midboss_TeleportOut : TeleportOutState
{
    private Stage1Midboss boss;
    private Stage1Midboss_Phase1 phase;

    public Stage1Midboss_TeleportOut(FiniteStateMachine stateMachine, Stage1Midboss boss, string animBoolName, Stage1Midboss_Phase1 phase) 
        : base(stateMachine, boss, animBoolName)
    {
        this.boss = boss;
        this.phase = phase;
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

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        AnimatorStateInfo animatorStateInfo = boss.CharacterAnimation.GetCurrentAnimatorStateInfo();
        if (animatorStateInfo.normalizedTime >= 1 && boss.CurrentBossPhase == phase)
        {
            boss.stateMachine.ChangeState(phase.teleportInState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

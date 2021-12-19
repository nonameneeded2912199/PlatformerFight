using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PlatformerFight.CharacterThings;

public class Stage1Midboss_PhaseTransition : IdleState
{
    private Stage1Midboss boss;

    public Stage1Midboss_PhaseTransition(FiniteStateMachine stateMachine, Stage1Midboss entity, string animBoolName, D_IdleState stateData = default) 
        : base(stateMachine, entity, animBoolName, stateData)
    {
        this.boss = entity;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        boss.SetInvincibility(true);
        boss.transform.DOMove(new Vector3(boss.centerPosition.position.x, boss.flightLevel1.transform.position.y, boss.transform.position.z), 0.5f)
            .OnComplete(() =>
            {
                Debug.Log("Phase 2 Start!");
                boss.NextBossPhase.StartPhase();
            });
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected D_IdleState stateData;

    protected bool flipAfterIdle;

    protected bool isIdleTimeOver;

    protected bool playerInMinAgroRangeCheck;

    protected float idleTime;

    public IdleState(FiniteStateMachine stateMachine, BaseEnemy entity, string animBoolName, D_IdleState stateData = default) 
        : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        playerInMinAgroRangeCheck = entity.CheckPlayerInMinArgoRange();
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(0f);
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (flipAfterIdle)
        {
            entity.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    private void SetRandomIdleTime()
    {
        if (stateData != null)
            idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}

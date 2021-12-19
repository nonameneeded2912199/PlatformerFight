using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportInState : State
{
    protected D_TeleportInStateData stateData;

    protected bool waitDone = false;

    public TeleportInState(FiniteStateMachine stateMachine, BaseEnemy entity, string animBoolName, D_TeleportInStateData stateData)
        : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        waitDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        if (Time.time >= startTime + stateData.waitTime)
        {
            waitDone = true;
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

using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected D_MoveState stateData;

    protected bool wallCheck;

    protected bool ledgeCheck;

    protected bool playerInMinAgroRangeCheck;

    public MoveState(FiniteStateMachine stateMachine, BaseEnemy entity, string animBoolName, D_MoveState stateData) 
        : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        ledgeCheck = entity.CheckLedge();
        wallCheck = entity.onWall;
        playerInMinAgroRangeCheck = entity.CheckPlayerInMinArgoRange();
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(stateData.movementSpeed);
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

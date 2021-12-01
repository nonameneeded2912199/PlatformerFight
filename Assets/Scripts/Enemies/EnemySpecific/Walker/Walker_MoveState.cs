using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker_MoveState : MoveState
{
    private Walker enemy;

    public Walker_MoveState(FiniteStateMachine stateMachine, Walker enemy, string animBoolName, D_MoveState stateData) :
        base(stateMachine, enemy, animBoolName, stateData)
    {
        this.enemy = enemy;
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (wallCheck)
        {
            if ((enemy.facingRight && enemy.onRightWall) || (!enemy.facingRight && !enemy.onRightWall))
                enemy.Flip();
            enemy.SetVelocity(stateData.movementSpeed);
        }

        if (!ledgeCheck && enemy.IsGrounded)
        {
            enemy.Flip();
            enemy.SetVelocity(stateData.movementSpeed);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

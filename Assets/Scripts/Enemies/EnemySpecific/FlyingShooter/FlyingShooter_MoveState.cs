using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingShooter_MoveState : MoveState
{
    private FlyingShooter enemy;
    private bool isPlayerInCircleRange;


    public FlyingShooter_MoveState(FiniteStateMachine stateMachine, FlyingShooter enemy, string animBoolName, D_MoveState stateData) :
        base(stateMachine, enemy, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInCircleRange = enemy.CheckPlayerInMaxRangedCircle();
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

        if (isPlayerInCircleRange)
        {
            stateMachine.ChangeState(enemy.rangedAttackState);
        }

        if (wallCheck)
        {
            if ((enemy.facingRight && enemy.onRightWall) || (!enemy.facingRight && !enemy.onRightWall))
                enemy.Flip();
            enemy.SetVelocity(stateData.movementSpeed);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_MoveToCenter : MoveState
{
    private Stage1Boss boss;
    private Transform centerPoint;

    public Stage1Boss_MoveToCenter(FiniteStateMachine stateMachine, Stage1Boss boss, string animBoolName, D_MoveState stateData, Transform centerPoint) :
        base(stateMachine, boss, animBoolName, stateData)
    {
        this.boss = boss;
        this.centerPoint = centerPoint;
    }

    public override void Enter()
    {
        base.Enter();

        int facingDirection = boss.facingRight ? 1 : -1;
        if ((centerPoint.position.x > boss.transform.position.x && facingDirection == -1)
            || (centerPoint.position.x < boss.transform.position.x && facingDirection == 1))
        {
            boss.Flip();
            boss.SetVelocity(stateData.movementSpeed);
        }
    }

    public override void Exit()
    {
        base.Exit();       
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (wallCheck || !ledgeCheck)
        {
            boss.Flip();
            boss.SetVelocity(stateData.movementSpeed);
        }

        if (boss.transform.position.x >= centerPoint.position.x - 1 && boss.transform.position.x <= centerPoint.position.x + 1)
        {
            stateMachine.ChangeState(boss.specialAttackFatal);
        }    
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

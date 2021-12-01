using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackState : State
{
    public KnockbackState(FiniteStateMachine stateMachine, BaseEnemy entity, string animBoolName) : base(stateMachine, entity, animBoolName)
    {
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        entity.Rigidbody.velocity = Vector2.zero;
        entity.Knockback(entity.facingRight ? -1 : 1);
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
        if (Time.time >= startTime + entity.KnockbackDuration)
        {
            entity.ResetKnockbackResistance();
            stateMachine.ChangeState(entity.reservedState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

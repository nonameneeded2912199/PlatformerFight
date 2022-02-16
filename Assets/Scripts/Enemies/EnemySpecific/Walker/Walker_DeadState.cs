using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker_DeadState : DeadState
{
    private Walker enemy;

    public Walker_DeadState(FiniteStateMachine stateMachine, Walker enemy, string animBoolName, D_DeadState stateData)
        : base(stateMachine, enemy, animBoolName, stateData)
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
        entity.gameObject.SetActive(false);

        GameObject.Destroy(entity.gameObject);
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

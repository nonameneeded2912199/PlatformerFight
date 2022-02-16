using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperJoe_DeadState : DeadState
{
    private SniperJoe enemy;

    public SniperJoe_DeadState(FiniteStateMachine stateMachine, SniperJoe enemy, string animBoolName, D_DeadState stateData)
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

        GameObject.Destroy(entity.gameObject); ;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected FiniteStateMachine stateMachine;
    protected BaseEnemy entity;
    public float startTime { get; private set; }

    protected string animBoolName;

    public State(FiniteStateMachine stateMachine, BaseEnemy entity, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.entity = entity;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        entity.CharacterAnimation.PlayAnim(animBoolName);
        DoChecks();
    }

    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void LateUpdate()
    {

    }

    public virtual void DoChecks()
    {

    }
}

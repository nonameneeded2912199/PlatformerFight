using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Stage1Boss_Pursue : MoveState
{
    private Stage1Boss boss;

    private Stage1Boss_Phase1 phase;

    private Player target;

    private float moveDuration = 0;

    private bool moved = false;

    public Stage1Boss_Pursue(FiniteStateMachine stateMachine, Stage1Boss entity, string animBoolName, D_MoveState stateData, Stage1Boss_Phase1 phase)
        : base(stateMachine, entity, animBoolName, stateData)
    {
        this.boss = entity;
        this.phase = phase;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        moved = false;
        moveDuration = Random.Range(stateData.minMoveTime, stateData.maxMoveTime);
        target = GameObject.FindObjectOfType<Player>();
        if (target == null)
            return;
        boss.aiPathfinder.StartPathFinding(target.transform, stateData.movementSpeed);
    }

    public override void Exit()
    {
        base.Exit();
        boss.aiPathfinder.StopPathFinding();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!boss.IsGrounded)
        {
            boss.CharacterAnimation.PlayAnim("Boss1_Jump / Fall");
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (boss.IsGrounded)
        {
            if (boss.Rigidbody.velocity.x != 0)
            {
                boss.CharacterAnimation.PlayAnim("Boss1_Move");
            }
            else
            {
                boss.CharacterAnimation.PlayAnim("Boss1_Idle");
            }
        }
    }
}

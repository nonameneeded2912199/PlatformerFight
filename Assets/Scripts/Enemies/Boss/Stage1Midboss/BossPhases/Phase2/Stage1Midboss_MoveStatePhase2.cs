using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stage1Midboss_MoveStatePhase2 : MoveState
{
    private Stage1Midboss boss;

    private Stage1Midboss_Phase2 phase;

    private Player target;

    private float moveDuration = 0;

    private bool moved = false;

    public Stage1Midboss_MoveStatePhase2(FiniteStateMachine stateMachine, Stage1Midboss entity, string animBoolName, D_MoveState stateData, Stage1Midboss_Phase2 phase) 
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

        float xGoal = target.transform.position.x;
        int facingDirection = boss.facingRight ? 1 : -1;

        if ((xGoal > boss.transform.position.x && facingDirection == -1) || (xGoal < boss.transform.position.x && facingDirection == 1))
        {
            boss.Flip();
            boss.SetVelocity(stateData.movementSpeed);
        }

        if (Time.time >= startTime + moveDuration && !moved)
        {
            moved = true;
            //boss.SetVelocity(0f);
            boss.transform.DOMove(new Vector3(boss.transform.position.x, boss.flightLevel2.position.y, boss.transform.position.z), 0.5f)
                .OnComplete(() =>
                {
                    stateMachine.ChangeState(phase.barrageState);
                });          
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

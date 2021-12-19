using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Midboss_TeleportIn : TeleportInState
{
    private Stage1Midboss boss;
    private Stage1Midboss_Phase1 phase;

    //private bool moved = false;

    private Player target;

    public Stage1Midboss_TeleportIn(FiniteStateMachine stateMachine, Stage1Midboss boss, string animBoolName, D_TeleportInStateData stateData, Stage1Midboss_Phase1 phase) 
        : base(stateMachine, boss, animBoolName, stateData)
    {
        this.boss = boss;
        this.phase = phase;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        target = GameObject.FindObjectOfType<Player>();
        float xGoal = target.transform.position.x;
        if (phase.randomShootTime < 3)
            boss.transform.position = new Vector3(xGoal, boss.flightLevel1.transform.position.y, boss.transform.position.z);
        else
            boss.transform.position = new Vector3(boss.centerPosition.position.x, boss.flightLevel2.position.y, boss.transform.position.z);
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
        }
        if (waitDone)
        {
            if (boss.CurrentBossPhase == phase)
            {
                if (phase.randomShootTime < 3)
                    boss.stateMachine.ChangeState(phase.spiralAttack);
                else
                    boss.stateMachine.ChangeState(phase.chaseAttack);
            }
        }       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

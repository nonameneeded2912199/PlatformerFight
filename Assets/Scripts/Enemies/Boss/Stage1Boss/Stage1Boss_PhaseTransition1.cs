using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stage1Boss_PhaseTransition1 : MoveState
{
    private Stage1Boss boss;

    private Transform phaseStartPos;

    private Transform phaseStartPos2;

    private Transform chosenPos;

    private Coroutine movingCoroutine;

    private Coroutine logicCoroutine;

    private bool isUpdating;

    private bool isPhysicsUpdating;

    public Stage1Boss_PhaseTransition1(FiniteStateMachine stateMachine, Stage1Boss entity, string animBoolName, D_MoveState stateData,
        Transform phaseStartPos, Transform phaseStartPos2)
        : base(stateMachine, entity, animBoolName, stateData)
    {
        this.boss = entity;
        this.phaseStartPos = phaseStartPos;
        this.phaseStartPos2 = phaseStartPos2;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        boss.SetInvincibility(true);

        float dis = Mathf.Abs(phaseStartPos.position.x - boss.transform.position.x);
        float dis2 = Mathf.Abs(phaseStartPos2.position.x - boss.transform.position.x);

        chosenPos = dis < dis2 ? phaseStartPos : phaseStartPos2;

        boss.aiPathfinder.StartPathFinding(chosenPos, stateData.movementSpeed);

        isPhysicsUpdating = true;
        isUpdating = true;

        movingCoroutine = boss.StartCoroutine(MovingCoroutine());
        logicCoroutine = boss.StartCoroutine(LogicCoroutine());
    }

    public override void Exit()
    {
        base.Exit();

        if (movingCoroutine != null)
            boss.StopCoroutine(movingCoroutine);
        if (logicCoroutine != null)
            boss.StopCoroutine(logicCoroutine);

        int facingDirection = boss.facingRight ? 1 : -1;
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if ((player.transform.position.x > player.transform.position.x && facingDirection == -1)
            || (player.transform.position.x < player.transform.position.x && facingDirection == 1))
        {
            boss.Flip();
        }
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (boss.transform.position.x <= chosenPos.position.x + 1 
            && boss.transform.position.x >= chosenPos.position.x - 1)
        {
            isUpdating = false;
            isPhysicsUpdating = false;

            boss.aiPathfinder.StopPathFinding();
            boss.SetVelocity(0f);

            boss.CharacterAnimation.PlayAnim("Boss1_Idle");

            boss.transform.position = new Vector3(chosenPos.position.x, boss.transform.position.y, boss.transform.position.z);

            boss.StartCoroutine(SwitchCoroutine());
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
        else
        {
            boss.CharacterAnimation.PlayAnim("Boss1_Jump / Fall");
        }
    }

    private IEnumerator MovingCoroutine()
    {
        while (isUpdating)
        {
            PhysicsUpdate();

            yield return null;
        }
    }

    private IEnumerator LogicCoroutine()
    {
        while (isPhysicsUpdating)
        {
            LogicUpdate();

            yield return null;
        }
    }

    private IEnumerator SwitchCoroutine()
    {
        Debug.Log("Switching");

        yield return new WaitForSeconds(2.5f);

        boss.NextBossPhase.StartPhase();
    }
}

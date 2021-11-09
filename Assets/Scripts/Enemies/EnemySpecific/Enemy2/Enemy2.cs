using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Entity
{
    public E2_MoveState moveState { get; private set; }

    public E2_IdleState idleState { get; private set; }

    public E2_PlayerDetectedState playerDetectedState { get; private set; }

    public E2_MeleeAttackState meleeAttackState { get; private set; }

    public E2_LookForPlayerState lookForPlayerState { get; private set; }

    public E2_StunState stunState { get; private set; }

    public E2_DeadState deadState { get; private set; }

    public E2_DodgeState dodgeState { get; private set; }

    public E2_RangedAttackState rangedAttackState { get; private set; }


    [SerializeField]
    private D_MoveState moveStateData;

    [SerializeField]
    private D_IdleState idleStateData;

    [SerializeField]
    private D_PlayerDetectedState playerDetectedStateData;

    [SerializeField]
    private D_MeleeAttackState meleeAttackStateData;

    [SerializeField]
    private D_LookForPlayerState lookForPlayerStateData;

    [SerializeField]
    private D_StunState stunStateData;

    [SerializeField]
    private D_DeadState deadStateData;

    [SerializeField]
    public D_DodgeState dodgeStateData;

    [SerializeField]
    private D_RangedAttackState rangedAttackStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    [SerializeField]
    private Transform rangedAttackPosition;

    protected override void Start()
    {
        base.Start();

        moveState = new E2_MoveState(stateMachine, this, "Move", moveStateData);
        idleState = new E2_IdleState(stateMachine, this, "Idle", idleStateData);
        playerDetectedState = new E2_PlayerDetectedState(stateMachine, this, "PlayerDetect", playerDetectedStateData);
        meleeAttackState = new E2_MeleeAttackState(stateMachine, this, "MeleeAttack", meleeAttackPosition, meleeAttackStateData);
        lookForPlayerState = new E2_LookForPlayerState(stateMachine, this, "LookForPlayer", lookForPlayerStateData);
        stunState = new E2_StunState(stateMachine, this, "Stun", stunStateData);
        deadState = new E2_DeadState(stateMachine, this, "Dead", deadStateData);
        dodgeState = new E2_DodgeState(stateMachine, this, "Dodge", dodgeStateData);
        rangedAttackState = new E2_RangedAttackState(stateMachine, this, "RangedAttack", meleeAttackPosition, rangedAttackStateData);

        stateMachine.Initialize(moveState);
    }

    public override void TakeDamage(AttackDetails attackDetails)
    {
        base.TakeDamage(attackDetails);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }

        else if (isStunned && stateMachine.CurrentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }

        else if (CheckPlayerInMinArgoRange())
        {
            stateMachine.ChangeState(rangedAttackState);
        }

        else if (!CheckPlayerInMinArgoRange())
        {
            lookForPlayerState.SetTurnImmediately(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
    public E1_IdleState idleState { get; private set; }
    public E1_MoveState moveState { get; private set; }

    public E1_PlayerDetectedState playerDetectedState { get; private set; }

    public E1_ChargeState chargeState { get; private set; }

    public E1_LookForPlayerState lookForPlayerState { get; private set; }

    public E1_MeleeAttackState meleeAttackState { get; private set; }

    public E1_StunState stunState { get; private set; }

    public E1_DeadState deadState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;

    [SerializeField]
    private D_MoveState moveStateData;

    [SerializeField]
    private D_PlayerDetectedState playerDetectedStateData;

    [SerializeField]
    private D_ChargeState chargeStateData;

    [SerializeField]
    private D_LookForPlayerState lookForPlayerStateData;

    [SerializeField]
    private D_MeleeAttackState meleeAttackStateData;

    [SerializeField]
    private D_StunState stunStateData;

    [SerializeField]
    private D_DeadState deadStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    protected override void Start()
    {
        base.Start();

        moveState = new E1_MoveState(stateMachine, this, "Move", moveStateData);
        idleState = new E1_IdleState(stateMachine, this, "Idle", idleStateData);
        playerDetectedState = new E1_PlayerDetectedState(stateMachine, this, "PlayerDetect", playerDetectedStateData);
        chargeState = new E1_ChargeState(stateMachine, this, "Charge", chargeStateData);
        lookForPlayerState = new E1_LookForPlayerState(stateMachine, this, "LookForPlayer", lookForPlayerStateData);
        meleeAttackState = new E1_MeleeAttackState(stateMachine, this, "MeleeAttack", meleeAttackPosition, meleeAttackStateData);
        stunState = new E1_StunState(stateMachine, this, "Stun", stunStateData);
        deadState = new E1_DeadState(stateMachine, this, "Dead", deadStateData);

        stateMachine.Initialize(moveState);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }

    public override void TakeDamage(AttackDetails attackDetails)
    {
        base.TakeDamage(attackDetails);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }

        if (isStunned && stateMachine.CurrentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingShooter : Entity
{
    public FlyingShooter_MoveState moveState { get; private set; }

    public FlyingShooter_DeadState deadState { get; private set; }

    public FlyingShooter_RangedAttackState rangedAttackState { get; private set; }

    [SerializeField]
    private D_MoveState moveStateData;

    [SerializeField]
    private D_DeadState deadStateData;

    [SerializeField]
    private D_RangedAttackState rangedAttackStateData;

    [SerializeField]
    private Transform rangedAttackPosition;

    [SerializeField]
    private Transform playerCircleDetector;

    protected override void Start()
    {
        base.Start();

        moveState = new FlyingShooter_MoveState(stateMachine, this, "Move", moveStateData);
        deadState = new FlyingShooter_DeadState(stateMachine, this, "Dead", deadStateData);
        rangedAttackState = new FlyingShooter_RangedAttackState(stateMachine, this, "RangedAttack", rangedAttackPosition, rangedAttackStateData);

        stateMachine.Initialize(moveState);
    }

    protected override void TakeDamage(AttackDetails attackDetails)
    {
        base.TakeDamage(attackDetails);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
    }

    public override void Respawn()
    {
        base.Respawn();
        stateMachine.Initialize(moveState);
        Debug.Log("Respawned");
    }
}

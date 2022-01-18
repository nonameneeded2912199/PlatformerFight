using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : BaseEnemy
{
    public Walker_MoveState moveState { get; private set; }

    public Walker_DeadState deadState { get; private set; }

    [SerializeField]
    private D_MoveState moveStateData;

    [SerializeField]
    private D_DeadState deadStateData;

    [SerializeField]
    protected VoidEventChannelSO OnDefeatedEvent;

    protected override void Start()
    {
        base.Start();

        moveState = new Walker_MoveState(stateMachine, this, "Move", moveStateData);
        deadState = new Walker_DeadState(stateMachine, this, "Dead", deadStateData);

        stateMachine.Initialize(moveState);
    }

    public override void Kill()
    {
        stateMachine.ChangeState(deadState);
    }

    protected override void TakeDamage(AttackDetails attackDetails)
    {
        base.TakeDamage(attackDetails);

        if (IsDead)
        {
            OnDefeatedEvent.RaiseEvent();
            OnAddScore.RaiseEvent(CalculateScoreAfterDefeat(entityData.scoreYield));
            Kill();
        }
    }

}

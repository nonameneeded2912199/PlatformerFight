using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperJoe : BaseEnemy
{
    public SniperJoe_LookForPlayerState lookForPlayerState { get; private set; }

    public SniperJoe_PlayerDetectedState playerDetectedState { get; private set; }

    public SniperJoe_RangedAttackState rangedAttackState { get; private set; }

    public SniperJoe_DeadState deadState { get; private set; }

    [SerializeField]
    private D_LookForPlayerState lookForPlayerData;

    [SerializeField]
    private D_DeadState deadStateData;

    [SerializeField]
    private D_PlayerDetectedState playerDetectedStateData;

    [SerializeField]
    private D_RangedAttackState rangedAttackStateData;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    public GameObject shield;

    [SerializeField]
    protected VoidEventChannelSO OnDefeatedEvent;

    protected override void Start()
    {
        base.Start();

        lookForPlayerState = new SniperJoe_LookForPlayerState(stateMachine, this, "Idle", lookForPlayerData);
        deadState = new SniperJoe_DeadState(stateMachine, this, "Dead", deadStateData);
        playerDetectedState = new SniperJoe_PlayerDetectedState(stateMachine, this, "Idle", playerDetectedStateData);
        rangedAttackState = new SniperJoe_RangedAttackState(stateMachine, this, "Shoot", attackPoint, rangedAttackStateData);

        stateMachine.Initialize(lookForPlayerState);
    }

    protected override void Update()
    {
        base.Update();
        //Debug.Log(stateMachine.CurrentState);
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

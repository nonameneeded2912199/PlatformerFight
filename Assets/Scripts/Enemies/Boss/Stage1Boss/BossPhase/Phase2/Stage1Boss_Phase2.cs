using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_Phase2 : BossPhase
{
    private Stage1Boss owner;

    private Stage1Boss_Phase2Data phaseData;

    public Stage1Boss_Phase2Initial InitialState { get; private set; }

    public Stage1Boss_Phase2Moving MovingState { get; private set; }

    public Stage1Boss_Phase2NormalShoot NormalShoot { get; private set; }

    public Stage1Boss_Phase2(Stage1Boss owner, Stage1Boss_Phase2Data phaseData, FloatEventChannelSO onPhaseTimerUpdate,
        PhaseResultEventChannelSO onCompletedPhase)
        : base(onPhaseTimerUpdate, onCompletedPhase)
    {
        this.owner = owner;
        this.phaseData = phaseData;

        InitialState = new Stage1Boss_Phase2Initial(owner.stateMachine, owner, "Boss1_Idle", phaseData.initialState, this);
        MovingState = new Stage1Boss_Phase2Moving(owner.stateMachine, owner, "Boss1_Roll", phaseData.movingState, phaseData.movingStateBullet,
            phaseData.movingStateBullet2, phaseData.movingStateBullet3, this);

        NormalShoot = new Stage1Boss_Phase2NormalShoot(owner.stateMachine, owner, "Boss1_Idle", owner.transform, this, phaseData.normalAttack);
    }

    public override void StartPhase()
    {
        owner.CurrentBossPhase = this;

        owner.CharacterStats.SetCharacterStats(phaseData.phaseStats);

        owner.stateMachine.ChangeState(InitialState);

        owner._OnBossStatusStart.RaiseEvent(phaseData);

        BeginCounting(phaseData);
    }

    public override void EndPhase()
    {
        switch (phaseData.phaseType)
        {
            case BossPhaseType.NormalAttack:
                owner.OnAddScore.RaiseEvent(owner.CalculateScoreAfterDefeat(phaseData.scoreYield));
                break;
            case BossPhaseType.TimeAttack:
                long finalScore = (long)((phaseTimeLeft * phaseData.scoreYield) / (phaseData.phaseBonusTime + phaseData.scoreYield));
                finalScore += phaseData.scoreYield;
                OnPhaseCompleted.RaiseEvent(phaseTimeLeft > 0, finalScore);
                owner.OnAddScore.RaiseEvent(owner.CalculateScoreAfterDefeat(finalScore));
                break;
        }
        owner.CurrentBossPhase = null;
        owner.NextBossPhase = owner.phase3;
        owner._OnBossStatusEnd.RaiseEvent();
        owner.stateMachine.ChangeState(owner.phaseTransition2);
    }

    public override void FixedUpdate(float deltaTime)
    {
        owner.stateMachine.CurrentState?.PhysicsUpdate();
    }

    public override void LateUpdate(float deltaTime)
    {
        base.LateUpdate(deltaTime);
        owner.stateMachine.CurrentState?.LateUpdate();
    }

    public override void TakeDamage(AttackDetails attackDetails)
    {
        float reduction = owner.CharacterStats.CurrentDefense / (owner.CharacterStats.CurrentDefense + 500);
        float multiplier = 1 - reduction;
        int incomingDMG = (int)(attackDetails.damageAmount * multiplier);

        owner.CharacterStats.CurrentHP -= incomingDMG;

        if (owner.CharacterStats.CurrentHP > 0)
        {
            owner.StartCoroutine(owner.BecomeInvincible(attackDetails.invincibleTime));

            owner.CreateDamagePopup(incomingDMG.ToString(), owner.transform.position);
        }
        else
        {
            EndPhase();
        }
    }

    public override void Update(float deltaTime)
    {
        owner.stateMachine.CurrentState?.LogicUpdate();
    }
}

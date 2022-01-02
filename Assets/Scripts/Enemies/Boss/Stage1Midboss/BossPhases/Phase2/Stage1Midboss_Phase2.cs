using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Midboss_Phase2 : BossPhase
{
    private Stage1Midboss owner;

    private Stage1Midboss_Phase2Data phaseData;

    public Stage1Midboss_InitialStatePhase2 initialState { get; private set; }

    public Stage1Midboss_MoveStatePhase2 moveState { get; private set; }

    public Stage1Midboss_BarrageState barrageState { get; private set; }

    public Stage1Midboss_Phase2(Stage1Midboss owner, Stage1Midboss_Phase2Data phaseData, FloatEventChannelSO onPhaseTimerUpdate,
        PhaseResultEventChannelSO onCompletedPhase)
        : base(onPhaseTimerUpdate, onCompletedPhase)
    {
        this.owner = owner;
        this.phaseData = phaseData;

        this.initialState = new Stage1Midboss_InitialStatePhase2(owner.stateMachine, owner, "Midboss1_Idle", phaseData.initialStateData, this);
        this.moveState = new Stage1Midboss_MoveStatePhase2(owner.stateMachine, owner, "Midboss1_Move", phaseData.moveStateData, this);

        this.barrageState = new Stage1Midboss_BarrageState(owner.stateMachine, owner, "Midboss1_Special", this, owner.transform, phaseData.barrageStateData);

    }

    public override void EndPhase()
    {
        switch (phaseData.phaseType)
        {
            case BossPhaseType.NormalAttack:
                owner.OnAddScore.RaiseEvent(owner.CalculateScoreAfterDefeat(phaseData.scoreYield));
                break;
            case BossPhaseType.SpellCardAttack:
                long finalScore = (long)((phaseTimeLeft * phaseData.scoreYield) / (phaseData.phaseBonusTime + phaseData.scoreYield));
                finalScore += phaseData.scoreYield;
                OnPhaseCompleted.RaiseEvent(phaseTimeLeft > 0, finalScore);
                owner.OnAddScore.RaiseEvent(owner.CalculateScoreAfterDefeat(finalScore));
                break;
        }    
        owner.IsDead = true;
        owner.CurrentBossPhase = null;
        owner._OnBossStatusEnd.RaiseEvent();
        owner.Kill();
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

    public override void StartPhase()
    {
        owner.CurrentBossPhase = this;

        owner._OnBossStatusStart.RaiseEvent(phaseData);

        owner.CharacterStats.SetCharacterStats(phaseData.phaseStats);

        owner.stateMachine.ChangeState(initialState);
        //owner.stateMachine.Initialize(initialState);

        BeginCounting(phaseData);
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

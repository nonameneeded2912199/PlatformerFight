using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_Phase3 : BossPhase
{
    private Stage1Boss owner;

    private Stage1Boss_Phase3Data phaseData;

    public Stage1Boss_Phase3Initial InitialState { get; private set; }

    public Stage1Boss_Phase3Barrage BarrageState { get; private set; }

    public Stage1Boss_Phase3(Stage1Boss owner, Stage1Boss_Phase3Data phaseData, FloatEventChannelSO onPhaseTimerUpdate, PhaseResultEventChannelSO onPhaseCompleted) 
        : base(onPhaseTimerUpdate, onPhaseCompleted)
    {
        this.owner = owner;
        this.phaseData = phaseData;

        InitialState = new Stage1Boss_Phase3Initial(owner.stateMachine, owner, "Boss1_Roll", phaseData.initialState, this);
        BarrageState = new Stage1Boss_Phase3Barrage(owner.stateMachine, owner, "Boss1_Roll", owner.transform, phaseData.barrageAttack, this);
    }

    public override void EndPhase()
    {
        long finalScore = 0;

        switch (phaseData.phaseType)
        {
            case BossPhaseType.NormalAttack:
                owner.OnAddScore.RaiseEvent(owner.CalculateScoreAfterDefeat(phaseData.scoreYield));
                break;
            case BossPhaseType.TimeAttack:
                finalScore = (long)((phaseTimeLeft * phaseData.scoreYield) / (phaseData.phaseBonusTime + phaseData.scoreYield));
                finalScore *= phaseData.scoreYield / 100;
                OnPhaseCompleted.RaiseEvent(phaseTimeLeft > 0, finalScore);
                owner.OnAddScore.RaiseEvent(owner.CalculateScoreAfterDefeat(finalScore));
                break;
            case BossPhaseType.TimeSurvival:
                finalScore = (long)((phaseData.scoreYield) / (phaseData.phaseBonusTime + phaseData.scoreYield));
                finalScore *= phaseData.scoreYield / 100;
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
        owner.SetInvincibility(true);
        owner.CurrentBossPhase = this;

        owner.CharacterStats.SetCharacterStats(phaseData.phaseStats);

        owner.stateMachine.ChangeState(InitialState);

        owner._OnBossStatusStart.RaiseEvent(phaseData);

        BeginCounting(phaseData);
    }

    public override void TakeDamage(AttackDetails attackDetails)
    {
        
    }

    public override void Update(float deltaTime)
    {
        owner.stateMachine.CurrentState?.LogicUpdate();
        if (phaseTimeLeft <= 0)
        {
            EndPhase();
        }    
    }
}

using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_Phase1 : BossPhase
{
    private Stage1Boss owner;

    private Stage1Boss_Phase1Data phaseData;

    public Stage1Boss_Pursue chasingPlayer { get; private set; }

    public int randomShootTime;


    public Stage1Boss_Phase1(Stage1Boss owner, Stage1Boss_Phase1Data phaseData, FloatEventChannelSO onPhaseTimerUpdate,
        PhaseResultEventChannelSO onCompletedPhase)
        : base(onPhaseTimerUpdate, onCompletedPhase)
    {
        this.owner = owner;
        this.phaseData = phaseData;
        chasingPlayer = new Stage1Boss_Pursue(owner.stateMachine, owner, "Boss1_Move", phaseData.pursueData, this);
    }

    public override void StartPhase()
    {
        owner.CurrentBossPhase = this;

        owner.CharacterStats.SetCharacterStats(phaseData.phaseStats);

        owner.stateMachine.Initialize(chasingPlayer);

        owner._OnBossStatusStart.RaiseEvent(phaseData);

        BeginCounting(phaseData);
    }

    public override void EndPhase()
    {
        owner.CurrentBossPhase = null;
        owner.OnAddScore.RaiseEvent(owner.CalculateScoreAfterDefeat(phaseData.scoreYield));
        //owner.NextBossPhase = owner.phase2;
        owner._OnBossStatusEnd.RaiseEvent();
        owner.stateMachine.ChangeState(owner.phaseTransition);
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

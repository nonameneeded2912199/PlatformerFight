using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Midboss_Phase1 : BossPhase
{
    private Stage1Midboss owner;

    private Stage1Midboss_Phase1Data phaseData;

    public Stage1Midboss_InitialStatePhase1 initialState { get; private set; }

    public Stage1Midboss_TeleportOut teleportOutState { get; private set; }

    public Stage1Midboss_TeleportIn teleportInState { get; private set; }

    public Stage1Midboss_SpiralAttack spiralAttack { get; private set; }

    public Stage1Midboss_DiamondChase chaseAttack { get; private set; }

    public int randomShootTime;
    

    public Stage1Midboss_Phase1(Stage1Midboss owner, Stage1Midboss_Phase1Data phaseData, FloatEventChannelSO onPhaseTimerUpdate, 
        PhaseResultEventChannelSO onCompletedPhase)
        :base(onPhaseTimerUpdate, onCompletedPhase)
    {
        this.owner = owner;
        this.phaseData = phaseData;
        initialState = new Stage1Midboss_InitialStatePhase1(this.owner.stateMachine, owner, "Midboss1_Idle", this.phaseData.initialStateData, this);

        teleportOutState = new Stage1Midboss_TeleportOut(this.owner.stateMachine, owner, "Midboss1_TeleportAway", this);

        teleportInState = new Stage1Midboss_TeleportIn(this.owner.stateMachine, owner, "Midboss1_TeleportIn", this.phaseData.teleportInStateData, this);

        spiralAttack = new Stage1Midboss_SpiralAttack(this.owner.stateMachine, owner, "Midboss1_RangedAttack", owner.transform, this, this.phaseData.spiralAttackStateData);

        chaseAttack = new Stage1Midboss_DiamondChase(this.owner.stateMachine, owner, "Midboss1_Special", owner.transform, this, this.phaseData.chasingAttackStateData);
    }

    public override void StartPhase()
    {
        owner.CurrentBossPhase = this;

        owner.CharacterStats.SetCharacterStats(phaseData.phaseStats);

        owner.stateMachine.Initialize(initialState);

        owner._OnBossStatusStart.RaiseEvent(phaseData);

        BeginCounting(phaseData);
    }

    public override void EndPhase()
    {
        owner.CurrentBossPhase = null;
        owner.OnAddScore.RaiseEvent(owner.CalculateScoreAfterDefeat(phaseData.scoreYield));
        owner.NextBossPhase = owner.phase2;
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

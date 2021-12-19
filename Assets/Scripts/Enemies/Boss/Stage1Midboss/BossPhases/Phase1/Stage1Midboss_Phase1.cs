using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Midboss_Phase1 : BossPhase
{
    private Stage1Midboss owner;

    private Stage1Midboss_Phase1Data stage1Midboss_Phase1Data;

    public Stage1Midboss_InitialStatePhase1 initialState { get; private set; }

    public Stage1Midboss_TeleportOut teleportOutState { get; private set; }

    public Stage1Midboss_TeleportIn teleportInState { get; private set; }

    public Stage1Midboss_SpiralAttack spiralAttack { get; private set; }

    public Stage1Midboss_DiamondChase chaseAttack { get; private set; }

    public int randomShootTime;
    

    public Stage1Midboss_Phase1(Stage1Midboss owner, Stage1Midboss_Phase1Data phaseData)
    {
        this.owner = owner;
        this.stage1Midboss_Phase1Data = phaseData;
        initialState = new Stage1Midboss_InitialStatePhase1(this.owner.stateMachine, owner, "Midboss1_Idle", stage1Midboss_Phase1Data.initialStateData, this);

        teleportOutState = new Stage1Midboss_TeleportOut(this.owner.stateMachine, owner, "Midboss1_TeleportAway", this);

        teleportInState = new Stage1Midboss_TeleportIn(this.owner.stateMachine, owner, "Midboss1_TeleportIn", stage1Midboss_Phase1Data.teleportInStateData, this);

        spiralAttack = new Stage1Midboss_SpiralAttack(this.owner.stateMachine, owner, "Midboss1_RangedAttack", owner.transform, this, stage1Midboss_Phase1Data.spiralAttackStateData);

        chaseAttack = new Stage1Midboss_DiamondChase(this.owner.stateMachine, owner, "Midboss1_Special", owner.transform, this, stage1Midboss_Phase1Data.chasingAttackStateData);
    }

    public void StartPhase()
    {
        owner.CurrentBossPhase = this;

        owner.CharacterStats.SetCharacterStats(stage1Midboss_Phase1Data.phaseStats);

        owner.stateMachine.Initialize(initialState);

        owner._OnBossStatusStart.RaiseEvent(stage1Midboss_Phase1Data);
    }

    public void EndPhase()
    {
        owner.CurrentBossPhase = null;
        owner.NextBossPhase = owner.phase2;
        owner._OnBossStatusEnd.RaiseEvent();
        owner.stateMachine.ChangeState(owner.phaseTransition);
    }

    public void FixedUpdate(float deltaTime)
    {
        owner.stateMachine.CurrentState?.PhysicsUpdate();
    }

    public void LateUpdate(float deltaTime)
    {
        owner.stateMachine.CurrentState?.LateUpdate();
    }

    public void TakeDamage(AttackDetails attackDetails)
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

    public void Update(float deltaTime)
    {
        owner.stateMachine.CurrentState?.LogicUpdate();
    }
}

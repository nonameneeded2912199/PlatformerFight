using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.CharacterThings
{
    public abstract class BossPhase
    {
        protected float phaseTimeLeft = 0;

        protected bool bonusCount = false;

        public FloatEventChannelSO OnPhaseTimerUpdate { get; protected set; }

        public PhaseResultEventChannelSO OnPhaseCompleted { get; protected set; }

        public BossPhase(FloatEventChannelSO onPhaseTimerUpdate, PhaseResultEventChannelSO onPhaseCompleted)
        {
            OnPhaseTimerUpdate = onPhaseTimerUpdate;
            OnPhaseCompleted = onPhaseCompleted;
        }

        public abstract void StartPhase();

        public abstract void Update(float deltaTime);

        public abstract void FixedUpdate(float deltaTime);

        public virtual void LateUpdate(float deltaTime)
        {
            if (phaseTimeLeft > 0 && bonusCount)
            {
                phaseTimeLeft -= deltaTime;
            }    
            else
            {
                phaseTimeLeft = 0;
            }

            OnPhaseTimerUpdate.RaiseEvent(phaseTimeLeft);
        }

        public abstract void TakeDamage(AttackDetails attackDetails);

        public abstract void EndPhase();

        protected void BeginCounting(BossPhaseDataSO phaseData)
        {
            if (phaseData.phaseType == BossPhaseType.NormalAttack)
                return;

            if (phaseData.phaseBonusTime > 0)
            {
                phaseTimeLeft = phaseData.phaseBonusTime;
                bonusCount = true;
            }    
        }
    }
}
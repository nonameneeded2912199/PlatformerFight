using PlatformerFight.CharacterThings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformerFight.Buffs
{
    public abstract class BaseBuff : AbilityEffect
    {
        protected BaseCharacter host;
        protected bool timed;
        protected float duration;
        protected int effectStacks;
        protected int maxStacks;

        public string buffName;
        public bool isFinished;

        public float durationRate { get => duration / Buff.Duration; }

        public GameObject BuffIconPrefab { get; set; }

        public Image IconSkill { get; set; }

        public Text EffectStackText { get; set; }

        public ScriptableBuff Buff { get; }

        public BaseBuff(ScriptableBuff buff, BaseCharacter host)
        {
            this.host = host;
            Buff = buff;
            timed = Buff.Timed;
            maxStacks = Buff.maxStacks;

            BuffIconPrefab = UnityEngine.Object.Instantiate(Buff.buffIconPrefab);

            IconSkill = BuffIconPrefab.GetComponentInChildren<Image>();
            IconSkill.sprite = Buff.icon;
            EffectStackText = BuffIconPrefab.GetComponentInChildren<Text>();
        }

        public void Tick(float deltaTime)
        {
            if (timed)
            {
                duration -= deltaTime;
                if (duration <= 0)
                {
                    duration = 0;
                    isFinished = true;
                }
            }
            IconSkill.fillAmount = durationRate;
        }

        public void Active(bool applyFirstStack)
        {
            if (applyFirstStack)
            {
                ApplyOnStart();
                effectStacks++;
                EffectStackText.text = effectStacks.ToString();
                duration += Buff.Duration;
            }

            if (Buff.IsEffectStacked && !applyFirstStack && effectStacks < maxStacks)
            {
                ApplyOnStart();
                effectStacks++;
                EffectStackText.text = effectStacks.ToString();
            }

            if (Buff.IsDurationStacked && !applyFirstStack)
            {
                duration += Buff.Duration;
            }
        }

        public abstract void ApplyOnStart();

        public abstract void ApplyOnEnd();

        public abstract void ApplyOnAttacking();
        public abstract void ApplyOnBeingAttacked();
        public abstract void ApplyOnDeath();
        public abstract void ApplyOnSkillAttacking();
        public abstract void ApplyOnUpdate();
    }
}
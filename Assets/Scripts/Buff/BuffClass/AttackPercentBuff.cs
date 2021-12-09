using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformerFight.Buffs
{
    public class AttackPercentBuff : BaseBuff
    {
        private readonly CharacterStats characterStats;

        public AttackPercentBuff(ScriptableBuff buff, BaseCharacter host)
            : base(buff, host)
        {
            characterStats = host.CharacterStats;
        }

        public override void ApplyOnAttacking()
        {

        }

        public override void ApplyOnBeingAttacked()
        {

        }

        public override void ApplyOnDeath()
        {

        }

        public override void ApplyOnEnd()
        {
            ScriptableAttackPercentBuff attackBuff = Buff as ScriptableAttackPercentBuff;
            characterStats.BonusAttack -= attackBuff.attackIncreasePercentage * characterStats.BaseATK / 100 * effectStacks;
            effectStacks = 0;
            Object.Destroy(BuffIconPrefab);
        }

        public override void ApplyOnSkillAttacking()
        {

        }

        public override void ApplyOnStart()
        {
            ScriptableAttackPercentBuff attackBuff = Buff as ScriptableAttackPercentBuff;
            characterStats.BonusAttack += attackBuff.attackIncreasePercentage * characterStats.BaseATK / 100;
        }

        public override void ApplyOnUpdate()
        {

        }
    }
}
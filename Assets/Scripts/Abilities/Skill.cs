using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterThings.Abilities
{
    public abstract class Skill
    {
        /// <summary>
        /// The one who will use this skill
        /// </summary>
        public BaseCharacter Executor { get; protected set; }

        /// <summary>
        /// Hitbox origin
        /// </summary>
        public Transform AttackPoint { get; protected set; }

        /// <summary>
        /// List of usable skill variations
        /// </summary>
        public List<SkillVariation> Variations { get; protected set; }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        public bool CanPerformOnAir { get; protected set; }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        public bool CanPerformOnGround { get; protected set; }

        public Sprite SkillIcon { get; protected set; }

        public ScriptableBuff[] BuffsToExecutor { get; protected set; }

        public ScriptableBuff[] BuffsInflict { get; protected set; }

        public string SkillName { get; protected set; }

        public SkillVariation currentVariation { get; protected set; }

        public float APCost { get; protected set; }

        public float CooldownTime { get; protected set; }
        protected float cooldownCount;

        #region Vector2 for rectangular hitbox
        public Vector2 topLeft => new Vector2(AttackPoint.position.x - currentVariation.horizontalSize / 2, AttackPoint.position.y + currentVariation.verticalSize / 2);

        public Vector2 topRight => new Vector2(AttackPoint.position.x + currentVariation.horizontalSize / 2, AttackPoint.position.y + currentVariation.verticalSize / 2);

        public Vector2 bottomLeft => new Vector2(AttackPoint.position.x - currentVariation.horizontalSize / 2, AttackPoint.position.y - currentVariation.verticalSize / 2);

        public Vector2 bottomRight => new Vector2(AttackPoint.position.x + currentVariation.horizontalSize / 2, AttackPoint.position.y - currentVariation.verticalSize / 2);

        #endregion

        public Skill(ScriptableSkill scriptableSkill, BaseCharacter executor, Transform attackPoint)
        {
            Executor = executor;
            AttackPoint = attackPoint;

            Variations = scriptableSkill.variations;
            CanPerformOnAir = scriptableSkill.canPerformOnAir;
            CanPerformOnGround = scriptableSkill.canPerformOnGround;
            SkillIcon = scriptableSkill.skillIcon;
            BuffsToExecutor = scriptableSkill.buffsToExecutor;
            BuffsInflict = scriptableSkill.buffsInflict;

            SkillName = scriptableSkill.skillName;
            APCost = scriptableSkill.apCost;
            CooldownTime = scriptableSkill.cooldownTime;
        }

        public bool CanPeform(bool isGrounded, BaseCharacter character)
        {
            if (isGrounded)
            {
                if (!CanPerformOnGround)
                    return false;
            }
            else
            {
                if (!CanPerformOnAir)
                    return false;
            }

            if (character.CharacterStats.CurrentAP < APCost)
            {
                return false;
            }

            if (cooldownCount > 0)
            {
                return false;
            }

            return true;
        }

        public float CooldownRate
        {
            get
            {
                if (CooldownTime > 0)
                {
                    return cooldownCount / CooldownTime;
                }
                else
                {
                    return 0;
                }
            }
        }

        public void EnterCooldown()
        {
            if (CooldownTime > 0)
                cooldownCount = CooldownTime;
        }

        public void CoolingDown(float deltaTime)
        {
            if (cooldownCount > 0)
            {
                cooldownCount -= deltaTime;
            }
            if (cooldownCount < 0)
            {
                cooldownCount = 0;
            }
        }

        public SkillVariation SetVariation(int index)
        {
            if (Variations.Count > 0)
            {
                if (index >= 0 && index < Variations.Count)
                {
                    currentVariation = Variations[index];
                    return currentVariation;
                }
                else
                {
                    currentVariation = Variations[0];
                    return currentVariation;
                }
            }

            return null;
        }

        public abstract bool Execute();

        public abstract void Damage();

        public virtual void PerformShot()
        {

        }

        public virtual void DrawGizmo()
        {
            if (currentVariation.circularHitbox)
                Gizmos.DrawWireSphere(AttackPoint.position, currentVariation.attackRadius);
            else
            {
                Gizmos.DrawLine(topLeft, topRight);
                Gizmos.DrawLine(topLeft, bottomLeft);
                Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.DrawLine(bottomLeft, bottomRight);
            }
        }
    }
}
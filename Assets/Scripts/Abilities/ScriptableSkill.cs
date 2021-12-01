using CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterThings.Abilities
{
    public abstract class ScriptableSkill : ScriptableObject
    {
        /// <summary>
        /// List of usable skill variations
        /// </summary>
        public List<SkillVariation> variations;

        public string skillName;

        /// <summary>
        /// Self-explanatory
        /// </summary>
        public bool canPerformOnAir = false;

        /// <summary>
        /// Self-explanatory
        /// </summary>
        public bool canPerformOnGround = true;

        public float apCost;

        public float cooldownTime;

        public Sprite skillIcon;

        public ScriptableBuff[] buffsToExecutor;

        public ScriptableBuff[] buffsInflict;      

        public abstract Skill InitializeSkill(BaseCharacter executor, Transform attackPoint);
    }
    /// <summary>
    /// Variation of the skill
    /// </summary>
    [System.Serializable]
    public class SkillVariation
    {
        public string animationName;

        /// <summary>
        /// Attack multiplier based on ATK or sth
        /// </summary>
        public float attackMultiplier;

        /// <summary>
        /// Bonus Knockback speed to receiver
        /// </summary>
        public Vector2 bonusKnockbackSpeed;

        /// <summary>
        /// Bonus Knockback time to receiver
        /// </summary>
        public float bonusKnockbackTime;

        /// <summary>
        /// Invincible time applied to target
        /// </summary>
        public float invicibleTime;

        public bool circularHitbox; // true: circular   false: rectangular  

        [ConditionalField("circularHitbox", true), Tooltip("Circular hitbox radius")]
        public float attackRadius;

        [ConditionalField("circularHitbox", false), Tooltip("Rectangular hitbox horitzontal size")]
        public float horizontalSize;
        [ConditionalField("circularHitbox", false), Tooltip("Rectangular hitbox vertical size")]
        public float verticalSize;

        public bool moveWhileExecuting;

        [ConditionalField("moveWhileExecuting", true), Tooltip("Rectangular hitbox vertical size")]
        public Vector2 movingVelocity;
    }
}

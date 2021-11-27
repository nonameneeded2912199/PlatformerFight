using CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Skill : ScriptableObject
{
    /// <summary>
    /// The one who will use this skill
    /// </summary>
    [HideInInspector]
    public BaseCharacter executor;

    /// <summary>
    /// Hitbox origin
    /// </summary>
    [HideInInspector]
    public Transform attackPoint;

    /// <summary>
    /// List of usable skill variations
    /// </summary>
    public List<SkillVariation> variations;

    /// <summary>
    /// The skill variation currently in use
    /// </summary>
    public SkillVariation currentVariation { get; protected set; }

    #region Vector2 for rectangular hitbox
    public Vector2 topLeft => new Vector2(attackPoint.position.x - currentVariation.horizontalSize / 2, attackPoint.position.y + currentVariation.verticalSize / 2);

    public Vector2 topRight => new Vector2(attackPoint.position.x + currentVariation.horizontalSize / 2, attackPoint.position.y + currentVariation.verticalSize / 2);

    public Vector2 bottomLeft => new Vector2(attackPoint.position.x - currentVariation.horizontalSize / 2, attackPoint.position.y - currentVariation.verticalSize / 2);

    public Vector2 bottomRight => new Vector2(attackPoint.position.x + currentVariation.horizontalSize / 2, attackPoint.position.y - currentVariation.verticalSize / 2);

    #endregion

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
    private float cooldownCount;

    public float cooldownRate
    {
        get
        {
            if (cooldownTime > 0)
            {
                return cooldownCount / cooldownTime;
            }
            else
            {
                return 0;
            }
        }
    }

    public Sprite skillIcon;

    public ScriptableBuff[] buffsToExecutor;

    public ScriptableBuff[] buffsInflict;

    /// <summary>
    /// Check if the skill can be used in ground, in mid-air, or both
    /// </summary>
    /// <param name="isGrounded"></param>
    /// <returns></returns>
    public bool CanPeform(bool isGrounded, BaseCharacter character)
    {
        if (isGrounded)
        {
            if (!canPerformOnGround)
                return false;
        }
        else
        {
            if (!canPerformOnAir)
                return false;
        }

        if (character.CharacterStats.CurrentAP < apCost)
        {
            return false;
        }

        if (cooldownCount > 0)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Set a variation of skill to use.
    /// </summary>
    /// <param name="index">Index of skill variation</param>
    /// <returns></returns>
    public SkillVariation SkillVariation(int index)
    {
        if (variations.Count > 0)
        {
            if (index >= 0 && index < variations.Count)
            {
                currentVariation = variations[index];
                return currentVariation;
            }
            else
            {
                currentVariation = variations[0];
                return currentVariation;
            }    
        }

        return null;
    }    

    public abstract bool Execute();

    public abstract void Damage();

    public virtual void DrawGizmo()
    {
        if (currentVariation.circularHitbox)
            Gizmos.DrawWireSphere(attackPoint.position, currentVariation.attackRadius);
        else
        {
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topLeft, bottomLeft);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomLeft, bottomRight);
        }
    }

    public virtual void PerformShot()
    {

    }

    public virtual void SetupBeforeSkill(BaseCharacter executor, Transform attackPoint)
    {
        this.executor = executor;
        this.attackPoint = attackPoint;
    }

    public void EnterCooldown()
    {
        if (cooldownTime > 0)
            cooldownCount = cooldownTime;
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

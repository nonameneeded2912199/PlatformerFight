using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Skill : ScriptableObject
{
    [HideInInspector]
    public BaseCharacter executor;

    [HideInInspector]
    public Transform attackPoint;

    public string skillName;

    public string animationName;

    public float cooldownTime;

    public float apCost;

    public float attackMultiplier;

    public bool canPerformOnAir = false;

    public bool canPerformOnGround = true;

    public bool CanPeform(bool isGrounded)
    {
        if (isGrounded)
        {
            if (canPerformOnGround)
                return true;
        }
        else
        {
            if (canPerformOnAir)
                return true;
        }

        return false;
    }

    public abstract void Execute();

    public abstract void Damage();

    public abstract void DrawGizmo();
}

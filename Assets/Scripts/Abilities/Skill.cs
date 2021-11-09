using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillData", menuName = "Skill/CharacterSkill")]
public class Skill : ScriptableObject
{
    public string skillName;

    public string animationName;

    public float cooldownTime;

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
                return false;
        }

        return false;
    }
}

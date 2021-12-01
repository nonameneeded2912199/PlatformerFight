using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AbilityEffect
{
    void ApplyOnStart();

    void ApplyOnUpdate();

    void ApplyOnAttacking();

    void ApplyOnSkillAttacking();

    void ApplyOnBeingAttacked();

    void ApplyOnEnd();

    void ApplyOnDeath();
}

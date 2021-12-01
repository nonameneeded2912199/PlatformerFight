using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackDetails
{
    public Vector2 position;
    public float damageAmount;
    public float invincibleTime;
    public float stunDamageAmount;
    public float increasedKnockbackTime;
    public Vector2 increasedKnockbackSpeed;
    public List<CharacterThings.BaseBuff> buffInflicts;
}

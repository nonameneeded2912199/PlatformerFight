using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test_Skill1Data", menuName = "Skill/TestCharacter/Skill1")]
public class Test_Skill1 : Skill
{
    public override void Damage()
    {
        Collider2D[] hitEnemies;
        if (executor.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (currentVariation.circularHitbox)
                hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, currentVariation.attackRadius, 1 << LayerMask.NameToLayer("Damagable"));
            else
                hitEnemies = Physics2D.OverlapAreaAll(topLeft, bottomRight, 1 << LayerMask.NameToLayer("Damagable"));

            AttackDetails attackDetails = new AttackDetails();
            attackDetails.position = executor.transform.position;
            attackDetails.damageAmount = executor.CharacterStats.BaseATK * currentVariation.attackMultiplier;
            attackDetails.invincibleTime = currentVariation.invicibleTime;
            attackDetails.stunDamageAmount = 0;
            foreach (Collider2D enemy in hitEnemies)
            {
                if (!enemy.gameObject.CompareTag(executor.gameObject.tag))
                    enemy.transform.SendMessage("TakeDamage", attackDetails);
            }
        }
    }

    public override bool Execute()
    {
        if (executor.CharacterStats.CurrentAP >= apCost)
        {
            SkillVariation(0);
            executor.Rigidbody.velocity = Vector2.zero;
            executor.characterAnimation.PlayAnim(currentVariation.animationName);
            executor.CharacterStats.ConsumeAP(apCost);
            return true;
        }
        return false;      
    }
}

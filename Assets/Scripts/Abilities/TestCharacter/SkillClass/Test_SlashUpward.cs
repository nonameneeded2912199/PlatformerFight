using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.Abilities
{
    public class Test_SlashUpward : Skill
    {
        public Test_SlashUpward(ScriptableSkill scriptableSkill, BaseCharacter executor, Transform attackPoint)
            : base(scriptableSkill, executor, attackPoint)
        {

        }

        public override void Damage()
        {
            Collider2D[] hitEnemies;
            if (Executor.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                HashSet<GameObject> ignoreList = new HashSet<GameObject>();

                int damagableLayer = 0;
                damagableLayer |= 1 << LayerMask.NameToLayer("Shield");
                damagableLayer |= 1 << LayerMask.NameToLayer("Damagable");

                if (currentVariation.circularHitbox)
                    hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, currentVariation.attackRadius, damagableLayer);
                else
                    hitEnemies = Physics2D.OverlapAreaAll(topLeft, bottomRight, damagableLayer);

                foreach (Collider2D hitEnemy in hitEnemies)
                {
                    if (hitEnemy.gameObject.layer == LayerMask.NameToLayer("Shield"))
                    {
                        ignoreList.Add(hitEnemy.gameObject);
                        ignoreList.Add(hitEnemy.transform.parent.gameObject);
                    }
                }

                AttackDetails attackDetails = new AttackDetails();
                attackDetails.position = Executor.transform.position;
                attackDetails.damageAmount = Executor.CharacterStats.CurrentAttack * currentVariation.attackMultiplier;
                attackDetails.increasedKnockbackSpeed = currentVariation.bonusKnockbackSpeed;
                attackDetails.increasedKnockbackTime = currentVariation.bonusKnockbackTime;
                attackDetails.invincibleTime = currentVariation.invicibleTime;
                attackDetails.stunDamageAmount = 0;
                foreach (Collider2D enemy in hitEnemies)
                {
                    if (ignoreList.Contains(enemy.gameObject))
                        continue;

                    if (!enemy.gameObject.CompareTag(Executor.gameObject.tag))
                        enemy.transform.SendMessage("TakeDamage", attackDetails);
                }
            }
        }

        public override bool Execute()
        {
            if (Executor.CharacterStats.CurrentAP >= APCost)
            {
                SetVariation(0);
                Executor.Rigidbody.velocity = Vector2.zero;
                Executor.CharacterAnimation.PlayAnim(currentVariation.animationName[0]);
                Executor.CharacterStats.ConsumeAP(APCost);
                Executor.CharacterBuffManager.AddBuff(BuffsToExecutor[0].InitializeBuff(Executor));
                if (currentVariation.moveWhileExecuting)
                    Executor.SetVelocity(currentVariation.movingVelocity);
                return true;
            }
            return false;
        }
    }
}
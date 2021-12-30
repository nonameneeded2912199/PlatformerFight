using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PlatformerFight.Abilities
{
    public class Test_DownwardThrust : Skill
    {
        private bool isThrusting;

        public Test_DownwardThrust(ScriptableSkill scriptableSkill, BaseCharacter executor, Transform attackPoint)
            :base(scriptableSkill, executor, attackPoint)
        {
            isThrusting = false;
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
                Executor.CharacterStats.ConsumeAP(APCost);
                SetVariation(0);
                if (Executor is Player)
                {
                    Player player = Executor as Player;
                    player.DisableMove();
                }
                if (Executor.IsGrounded)
                {
                    Executor.Rigidbody.gravityScale = 0;
                    Executor.CharacterAnimation.PlayAnim("Jump / Fall");
                    Vector3 endJump = Executor.transform.position;
                    endJump.y += 2f;

                    Executor.Rigidbody.DOJump(endJump, 1, 1, 0.5f).OnComplete(() =>
                    {
                        ExecuteThrust();
                    });
                }
                else
                {
                    ExecuteThrust();
                }
                return true;
            }
            return false;
        }

        private void ExecuteThrust()
        {
            isThrusting = true;
            Executor.SetInvincibility(true);
            Executor.Rigidbody.gravityScale = Executor.CharacterStats.OriginalGravityScale * 1.5f;
            Executor.CharacterAnimation.PlayAnim(currentVariation.animationName[0]);           
            if (currentVariation.moveWhileExecuting)
                Executor.SetVelocity(currentVariation.movingVelocity);
        }

        public override void SkillCancel()
        {
        }

        public override void SkillEnd()
        {
        }

        public override void SkillUpdate(float deltaTime)
        {
            if (Executor.IsGrounded && isThrusting)
            {
                isThrusting = false;
                Executor.SetInvincibility(false);
                Executor.CharacterAnimation.PlayAnim(currentVariation.animationName[1]);
                Executor.Rigidbody.gravityScale = Executor.CharacterStats.OriginalGravityScale;
            }
        }
    }
}
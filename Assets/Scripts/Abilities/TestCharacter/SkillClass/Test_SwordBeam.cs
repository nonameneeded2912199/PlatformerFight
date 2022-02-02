using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.Abilities
{
    public class Test_SwordBeam : Skill
    {
        public BulletDetails SwordBeamDetail { get; private set; }

        private BulletEventChannelSO bulletEventChannel = default;

        public Test_SwordBeam(ScriptableSkill scriptableSkill, BaseCharacter executor, Transform attackPoint, BulletDetails swordBeamDetail, BulletEventChannelSO bulletEventChannel)
            : base(scriptableSkill, executor, attackPoint)
        {
            SwordBeamDetail = swordBeamDetail;
            this.bulletEventChannel = bulletEventChannel;
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

        public override void PerformShot()
        {
            float direction = 0;
            if (currentVariation == Variations[0])
            {
                direction = Executor.facingRight ? 0 : Mathf.PI;
            }
            else
            {
                direction = Executor.facingRight ? -Mathf.PI / 4 : -3 * Mathf.PI / 4;
            }

            bulletEventChannel.RaiseBulletEvent(Executor.tag, AttackPoint.position, SwordBeamDetail.bulletSpeed, direction, SwordBeamDetail.bulletAcceleration,
                SwordBeamDetail.bulletLifeSpan, SwordBeamDetail.damageMultiplier * Executor.CharacterStats.CurrentAttack, 0.5f, SwordBeamDetail.hitRadius,
                SwordBeamDetail.bulletSprite, SwordBeamDetail.animatorOverrideController, 0, false, true, false);
        }

        public override bool Execute()
        {
            if (Executor.IsGrounded)
            {
                SetVariation(0);
                //executor.Rigidbody.velocity = Vector2.zero;
            }
            else SetVariation(1);
            if (Executor.CharacterStats.CurrentAP >= APCost)
            {
                if (currentVariation == Variations[0])
                    Executor.Rigidbody.velocity = Vector2.zero;
                if (currentVariation.moveWhileExecuting)
                    Executor.SetVelocity(currentVariation.movingVelocity);
                Executor.PlayableDirector.Stop();
                Executor.PlayableDirector.Play(currentVariation.animationPlayable[0]);
                Executor.CharacterStats.ConsumeAP(APCost);
                return true;
            }
            return false;
        }

        public override void SkillUpdate(float deltaTime)
        {
            
        }

        public override void SkillEnd()
        {
            
        }

        public override void SkillCancel()
        {
            
        }
    }
}
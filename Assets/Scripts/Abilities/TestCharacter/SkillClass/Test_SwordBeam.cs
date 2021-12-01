using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterThings.Abilities
{
    public class Test_SwordBeam : Skill
    {
        public float BulletMultiplier { get; private set; }

        public GameObject SwordbeamOBJ { get; private set; }

        public Test_SwordBeam(ScriptableSkill scriptableSkill, BaseCharacter executor, Transform attackPoint, float BulletMultiplier, GameObject SwordbeamOBJ)
            : base(scriptableSkill, executor, attackPoint)
        {
            this.BulletMultiplier = BulletMultiplier;
            this.SwordbeamOBJ = SwordbeamOBJ;
        }

        public override void Damage()
        {
            Collider2D[] hitEnemies;
            if (Executor.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                HashSet<GameObject> ignoreList = new HashSet<GameObject>();

                int damagableLayer = 0;
                damagableLayer |= (1 << LayerMask.NameToLayer("Shield"));
                damagableLayer |= (1 << LayerMask.NameToLayer("Damagable"));

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

            var bullet = PoolManager.SpawnObject(SwordbeamOBJ).GetComponent<Bullet>();
            bullet.SetAllegiance(Executor.tag);
            bullet.SetAttributes(AttackPoint.position, 8, direction, 0, 0, BulletMultiplier * Executor.CharacterStats.BaseATK, 0.5f);

            //GameObject bullet = Bullet.GetBullet(BulletOwner.Player, attackPoint.position, 8, direction, 0, bulletMultiplier * executor.CharacterStats.BaseATK, BulletType.Arrow,
            //    BulletColor.GREEN);
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
                Executor.CharacterAnimation.PlayAnim(currentVariation.animationName);
                Executor.CharacterStats.ConsumeAP(APCost);
                return true;
            }
            return false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test_SwordBeamData", menuName = "Skill/TestCharacter/SwordBeam")]
public class Test_SwordBeam : Skill
{
    public float bulletMultiplier;

    public GameObject swordbeamOBJ;
    

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

    public override void PerformShot()
    {
        float direction = 0;
        if (currentVariation == variations[0])
        {
            direction = executor.facingRight ? 0 : Mathf.PI;
        }
        else
        {
            direction = executor.facingRight ? -Mathf.PI / 4 : -3 * Mathf.PI / 4;
        }

        var bullet = PoolManager.SpawnObject(swordbeamOBJ).GetComponent<Bullet>();
        bullet.SetAllegiance(executor.tag);
        bullet.SetAttributes(attackPoint.position, 8, direction, 0, 0, bulletMultiplier * executor.CharacterStats.BaseATK, 0.5f);

        //GameObject bullet = Bullet.GetBullet(BulletOwner.Player, attackPoint.position, 8, direction, 0, bulletMultiplier * executor.CharacterStats.BaseATK, BulletType.Arrow,
        //    BulletColor.GREEN);
    }

    public override bool Execute()
    {
        if (executor.IsGrounded)
        {
            SkillVariation(0);
            //executor.Rigidbody.velocity = Vector2.zero;
        }
        else SkillVariation(1);
        if (executor.CharacterStats.CurrentAP >= apCost)
        {
            if (currentVariation == variations[0])
                executor.Rigidbody.velocity = Vector2.zero;
            executor.characterAnimation.PlayAnim(currentVariation.animationName);
            executor.CharacterStats.ConsumeAP(apCost);
            return true;
        }
        return false;
    }
}

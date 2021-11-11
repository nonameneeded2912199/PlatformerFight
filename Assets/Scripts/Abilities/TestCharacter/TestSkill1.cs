using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill1 : Skill
{
    public float attackRadius;

    public float invicibleTime;

    public override void Execute()
    {
        executor.characterAnimation.PlayAnim(animationName);
    }

    public override void Damage()
    {
        if (executor.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, LayerMask.NameToLayer("Damageable"));

            foreach (Collider2D enemy in hitEnemies)
            {
                if (!enemy.gameObject.CompareTag(executor.gameObject.tag))
                    enemy.transform.SendMessage("TakeDamage");
            }
        }
    }

    public override void DrawGizmo()
    {
        Gizmos.DrawSphere(attackPoint.position, attackRadius);
    }
}

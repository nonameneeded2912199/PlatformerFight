using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test_NormalATK2Data", menuName = "Skill/TestCharacter/NormalATK2")]
public class Test_NormalATK2 : Skill
{
    public float attackVerticalRadius;

    public float attackHorizontalRadius;

    public float invicibleTime;

    public Vector2 topLeft => new Vector2(attackPoint.position.x - attackHorizontalRadius / 2, attackPoint.position.y + attackVerticalRadius / 2);

    public Vector2 topRight => new Vector2(attackPoint.position.x + attackHorizontalRadius / 2, attackPoint.position.y + attackVerticalRadius / 2);

    public Vector2 bottomLeft => new Vector2(attackPoint.position.x - attackHorizontalRadius / 2, attackPoint.position.y - attackVerticalRadius / 2);

    public Vector2 bottomRight => new Vector2(attackPoint.position.x + attackHorizontalRadius / 2, attackPoint.position.y - attackVerticalRadius / 2);

    public override void Execute()
    {
        executor.characterAnimation.PlayAnim(animationName);
    }

    public override void Damage()
    {
        if (executor.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Collider2D[] hitEnemies = Physics2D.OverlapAreaAll(topLeft, bottomRight, LayerMask.NameToLayer("Damageable"));

            foreach (Collider2D enemy in hitEnemies)
            {
                if (!enemy.gameObject.CompareTag(executor.gameObject.tag))
                    enemy.transform.SendMessage("TakeDamage");
            }
        }
    }

    public override void DrawGizmo()
    {
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomLeft, bottomRight);
    }
}

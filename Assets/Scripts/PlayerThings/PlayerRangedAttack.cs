using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRangedAttack : PlayerAttack
{
    // Start is called before the first frame update

    public int bulletLimit = 3;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.normalizedTime >= 0.9f && IsAttackingAnimation())
        {
            isAttacking = false;
            canAttack = true;
            playerMovement.EnableMove();
        }
    }

    public override void NormalAttack(InputAction.CallbackContext context)
    {
        if (!playerMovement.IsDashing && canAttack /*&& !isAttacking*/)
        {
            canAttack = false;
            rb.velocity = Vector2.zero;
            isAttacking = true;
            playerAnimation.PlayAnim("NormalATKShoot");
        }
    }

    protected override bool IsAttackingAnimation()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("NormalATKShoot");
    }

    public void PerformShot()
    {
        Bullet.GetBullet(BulletOwner.Player, AttackPoint.position, 3, playerMovement.facingRight ? 0 : Mathf.PI,
            0, 5, BulletType.Kunai, BulletColor.RED);
    }
}

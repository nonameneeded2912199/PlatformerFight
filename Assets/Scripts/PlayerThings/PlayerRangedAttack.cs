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
        animatorStateInfo = playerAnimation.GetCurrentAnimatorStateInfo();
        if (animatorStateInfo.normalizedTime >= 0.9f && IsNormalAttacking())
        {
            isAttacking = false;
            canAttack = true;
            player.EnableMove();
        }
    }

    public override void NormalAttack(InputAction.CallbackContext context)
    {
        if (!player.IsDashing && canAttack /*&& !isAttacking*/)
        {
            canAttack = false;
            rb.velocity = Vector2.zero;
            isAttacking = true;
            playerAnimation.PlayAnim("NormalATKShoot");
        }
    }

    protected override bool IsNormalAttacking()
    {
        return playerAnimation.GetCurrentAnimatorStateInfo().IsName("NormalATKShoot");
    }

    public void PerformShot()
    {
        Bullet.GetBullet(BulletOwner.Player, AttackPoint.position, 3, player.facingRight ? 0 : Mathf.PI,
            0, 5, BulletType.Kunai, BulletColor.RED);
    }
}

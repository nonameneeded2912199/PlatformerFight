using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeCombat : PlayerAttack
{
    [SerializeField]
    private int normalATKCombo = 0;

    [SerializeField]
    private int normalATKCount;

    #region Properties

    public int NormalATKCount => normalATKCount;

    public int Combo => normalATKCombo;

    #endregion

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        animatorStateInfo = playerAnimation.GetCurrentAnimatorStateInfo();
        if (animatorStateInfo.normalizedTime >= 0.9f && IsAttackingAnimation() && !player.CanMove)
        {
            normalATKCombo = 0;
            player.EnableMove();
        }
        if (rb.velocity != Vector2.zero)
        {
            normalATKCombo = 0;
        }
        if (normalATKCombo == 0)
        {
            isAttacking = false;
        }

        /*if (!IsAttackingAnimation() && !player.CanMove)
        {
            player.EnableMove();
        }*/
    }

    /*protected override bool IsAttackingAnimation()
    {
        for (int i = 1; i <= normalATKCount; i++)
        {
            if (animatorStateInfo.IsName("NormalATK" + i))
            {
                return true;
            }
        }
        return false;
    }*/

    public override void NormalAttack(InputAction.CallbackContext context)
    {
        if (!player.IsDashing && canAttack /*&& !isAttacking*/ && normalATKCombo < normalAttack.Count)
        {
            if (player.IsGrounded)
            {
                animatorStateInfo = playerAnimation.GetCurrentAnimatorStateInfo();
                if (normalATKCombo == 0 || (IsNormalAttacking() && IsAttackingAnimation() && animatorStateInfo.normalizedTime < 0.8f))
                {
                    isAttacking = true;
                    normalATKCombo++;
                    normalATKCombo = Mathf.Clamp(normalATKCombo, 1, normalAttack.Count);
                    currentSkill = normalAttack[normalATKCombo - 1];
                    SetupBeforeSkill();
                    if (currentSkill.CanPeform(player.IsGrounded))
                        currentSkill.Execute();
                    //playerAnimation.PlayAnim("NormalATK" + normalATKCombo);
                }
            }
            else
            {
                isAttacking = true;
                currentSkill = normalJumpAttack;
                SetupBeforeSkill();
                if (currentSkill.CanPeform(player.IsGrounded))
                    currentSkill.Execute();
            }

        }
    }
}
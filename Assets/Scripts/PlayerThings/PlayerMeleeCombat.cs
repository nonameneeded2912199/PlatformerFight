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
        animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.normalizedTime >= 0.9f && IsAttackingAnimation())
        {
            normalATKCombo = 0;
            playerMovement.EnableMove();
        }
        if (rb.velocity != Vector2.zero)
        {
            normalATKCombo = 0;
        }
        if (normalATKCombo == 0)
        {
            isAttacking = false;
        }

        if (!IsAttackingAnimation())
        {
            playerMovement.EnableMove();
        }
    }

    protected override bool IsAttackingAnimation()
    {
        for (int i = 1; i <= normalATKCount; i++)
        {
            if (animatorStateInfo.IsName("NormalATK" + i))
            {
                return true;
            }
        }
        return false;
    }

    public override void NormalAttack(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<float>());
        if (!playerMovement.IsDashing && canAttack /*&& !isAttacking*/)
        {
            //Debug.Log(context.ReadValue<>());
            //canAttack = true;  
            if (playerMovement.IsGrounded)
            {
                animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
                if (normalATKCombo == 0 || (IsAttackingAnimation() && animatorStateInfo.normalizedTime < 0.8f))
                {
                    rb.velocity = Vector2.zero;
                    isAttacking = true;
                    normalATKCombo++;
                    playerAnimation.PlayAnim("NormalATK" + normalATKCombo);
                    normalATKCombo = Mathf.Clamp(normalATKCombo, 0, normalATKCount);
                }               
            }
            else
            {
                isAttacking = true;
                playerAnimation.PlayAnim("JumpNormalATK");
            }
            
        }
    }
}

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
        if (animatorStateInfo.normalizedTime >= 0.9f && IsNormalAttacking())
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

        if (!IsNormalAttacking())
        {
            playerMovement.EnableMove();
        }
    }

    protected override bool IsNormalAttacking()
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
        if (!playerMovement.IsDashing && canAttack /*&& !isAttacking*/ && normalATKCombo < normalATKCount)
        {
            //Debug.Log(context.ReadValue<>());
            //canAttack = true;  
            if (playerMovement.IsGrounded)
            {
                animatorStateInfo = playerAnimation.GetCurrentAnimatorStateInfo();
                if (normalATKCombo == 0 || (IsNormalAttacking() && animatorStateInfo.normalizedTime < 0.8f))
                {
                    rb.velocity = Vector2.zero;
                    isAttacking = true;
                    normalATKCombo++;
                    normalATKCombo = Mathf.Clamp(normalATKCombo, 1, normalATKCount);
                    playerAnimation.PlayAnim("NormalATK" + normalATKCombo);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerAttack : MonoBehaviour
{
    [Header("Component")]
    protected Rigidbody2D rb;
    protected Player playerMovement;
    protected PlayerAnimation playerAnimation;
    protected Animator animator;

    protected AnimatorStateInfo animatorStateInfo;

    private PlayerInputAction playerInputAction;

    [SerializeField]
    protected bool isAttacking;

    [SerializeField]
    protected bool canAttack = true;

    public bool IsAttacking => isAttacking;

    public Skill skill1;
    public Skill skill2;
    public Skill skill3;
    public Skill skill4;

    public Transform AttackPoint;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();

        playerInputAction.Player.NormalATK.performed += ctx => NormalAttack(ctx);
        playerInputAction.Enable();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<Player>();
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
        canAttack = true;
    }

    public abstract void NormalAttack(InputAction.CallbackContext context);

    protected abstract bool IsAttackingAnimation();

    public void SkillButton1(InputAction.CallbackContext context)
    {
        if (skill1 != null && context.performed && !isAttacking)
        {
            if (skill1.CanPeform(playerMovement.IsGrounded))
            {
                isAttacking = true;
                rb.velocity = Vector2.zero;
                playerAnimation.PlayAnim(skill1.animationName);
            }
        }
    }

    public void SkillButton2(InputAction.CallbackContext context)
    {
        if (skill2 != null && context.performed && !isAttacking)
        {
            if (skill2.CanPeform(playerMovement.IsGrounded))
            {
                isAttacking = true;
                rb.velocity = Vector2.zero;
                playerAnimation.PlayAnim(skill2.animationName);
            }
        }
    }

    public void SkillButton3(InputAction.CallbackContext context)
    {
        if (skill3 != null && context.performed && !isAttacking)
        {
            if (skill3.CanPeform(playerMovement.IsGrounded))
            {
                isAttacking = true;
                rb.velocity = Vector2.zero;
                playerAnimation.PlayAnim(skill3.animationName);
            }
        }
    }

    public void SkillButton4(InputAction.CallbackContext context)
    {
        if (skill4 != null && context.performed && !isAttacking)
        {
            if (skill4.CanPeform(playerMovement.IsGrounded))
            {
                isAttacking = true;
                rb.velocity = Vector2.zero;
                playerAnimation.PlayAnim(skill4.animationName);
            }
        }
    }

    public void SetAttackState(bool state)
    {
        isAttacking = state;
    }
}

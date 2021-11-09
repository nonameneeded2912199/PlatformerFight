using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Player playerMovement;
    private PlayerAttack playerCombat;
    private Rigidbody2D rb;

    private bool isPlayingTillEnd;

    public string currentState;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<Player>();
        playerCombat = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Jump", rb.velocity.y);
        if (playerCombat.IsAttacking)
            return;
        if (!playerMovement.IsDashing)
        {
            if (playerMovement.IsGrounded)
            {
                if (playerMovement.Horizontal != 0)
                {
                    PlayAnim("Move");
                }
                else
                {
                    PlayAnim("Idle");
                }
            }
            else
            {
                if (playerMovement.WallSlide)
                {
                    PlayAnim("WallGrab");
                }
                else
                {
                    PlayAnim("Jump / Fall");
                }
            }
        }
        else
        {
            PlayAnim("Dash");
        }
    }

    public void PlayAnim(string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);
        currentState = newState;
    }

    public void ResetAnimationState(string newState)
    {
        animator.Play(newState, 0, 0f);
        currentState = newState;
    }

    private IEnumerator PlayAttackAnimationTillEnd(string newState)
    {
        if (currentState == newState)
            yield return null;

        animator.Play(newState);
        currentState = newState;

        yield return new WaitForEndOfFrame();

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && 
            animator.GetCurrentAnimatorStateInfo(0).IsName(newState))
        {
            yield return new WaitForEndOfFrame();
        }

        playerCombat.SetAttackState(false);
    }
    
    public void SetAttackAnimation(string newState)
    {
        StartCoroutine(PlayAttackAnimationTillEnd(newState));
    }
}

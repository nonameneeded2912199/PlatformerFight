using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.CharacterThings
{
    public class CharacterAnimation : MonoBehaviour
    {
        public Animator animator { get; private set; }
        public Rigidbody2D rb { get; private set; }

        private bool isPlayingTillEnd;

        public string currentState;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
            animator.SetFloat("YVelocity", rb.velocity.y);
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

        protected IEnumerator PlayAttackAnimationTillEnd(string newState)
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

            //playerCombat.SetAttackState(false);
        }

        public AnimatorStateInfo GetCurrentAnimatorStateInfo()
        {
            return animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}
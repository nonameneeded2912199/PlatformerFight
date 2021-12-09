using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.CharacterThings
{
    public class PlayerAnimation : CharacterAnimation
    {
        private Player player;
        private PlayerAttack playerCombat;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<Player>();
            playerCombat = GetComponent<PlayerAttack>();
        }

        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            animator.SetFloat("YVelocity", rb.velocity.y);
            if (playerCombat.IsAttacking)
                return;
            if (player.IsKnockback)
            {
                PlayAnim("Hurt");
            }
            else
            {
                if (!player.IsDashing)
                {
                    if (player.IsGrounded)
                    {
                        if (player.Horizontal != 0)
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
                        if (player.WallSlide)
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
        }

        public void SetAttackAnimation(string newState)
        {
            StartCoroutine(PlayAttackAnimationTillEnd(newState));
        }
    }
}
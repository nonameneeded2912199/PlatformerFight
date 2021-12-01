using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using CharacterThings.Abilities;

namespace CharacterThings
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Component")]
        protected Rigidbody2D rb;
        protected Player player;
        protected PlayerAnimation playerAnimation;

        protected AnimatorStateInfo animatorStateInfo;

        private PlayerInputAction playerInputAction;

        [SerializeField]
        protected bool isAttacking;

        [SerializeField]
        protected bool isSkillAttacking;

        [SerializeField]
        protected bool canAttack = true;

        public bool IsAttacking => isAttacking;

        [Header("Skills")]
        [SerializeField]
        protected ScriptableSkill normalATKScriptable;

        [SerializeField]
        protected Skill normalAttack;

        [SerializeField]
        private int normalATKCombo = 0;

        public int Combo => normalATKCombo;

        [SerializeField]
        protected ScriptableSkill normalAirATKScriptable;

        [SerializeField]
        protected Skill normalAirAttack;

        public Skill skill1;
        public Skill skill2;
        public Skill skill3;
        public Skill skill4;

        public Image imageSkill1;
        public Image imageSkill2;
        public Image imageSkill3;
        public Image imageSkill4;

        [SerializeField]
        protected List<Skill> listSkill;

        [SerializeField]
        protected List<ScriptableSkill> presetSkill;

        [SerializeField]
        protected Skill currentSkill;

        public Transform AttackPoint;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GetComponent<Player>();
            playerAnimation = GetComponent<PlayerAnimation>();

            listSkill = new List<Skill>();

            normalAttack = normalATKScriptable.InitializeSkill(player, AttackPoint);

            normalAirAttack = normalAirATKScriptable.InitializeSkill(player, AttackPoint);

            foreach (ScriptableSkill scriptableSkill in presetSkill)
            {
                listSkill.Add(scriptableSkill.InitializeSkill(player, AttackPoint));
            }

            skill1 = listSkill[0];
            skill2 = listSkill[1];
            skill3 = listSkill[2];
            skill4 = listSkill[0];

            if (skill1 != null)
                imageSkill1.sprite = skill1.SkillIcon;

            if (skill2 != null)
                imageSkill2.sprite = skill2.SkillIcon;

            if (skill3 != null)
                imageSkill3.sprite = skill3.SkillIcon;

            if (skill4 != null)
                imageSkill4.sprite = skill4.SkillIcon;

            playerInputAction = new PlayerInputAction();

            playerInputAction.Player.NormalATK.performed += ctx => NormalAttackButton(ctx);
            playerInputAction.Player.Skill1.performed += ctx => SkillButton1(ctx);
            playerInputAction.Player.Skill2.performed += ctx => SkillButton2(ctx);
            playerInputAction.Player.Skill3.performed += ctx => SkillButton3(ctx);
            playerInputAction.Player.Skill4.performed += ctx => SkillButton4(ctx);
            playerInputAction.Enable();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            canAttack = true;
        }

        private void Update()
        {
            animatorStateInfo = playerAnimation.GetCurrentAnimatorStateInfo();
            if (player.IsKnockback && IsAttacking)
            {
                if (isSkillAttacking)
                    isSkillAttacking = false;

                if (isAttacking)
                    isAttacking = false;

                normalATKCombo = 0;

                StartCoroutine(CanRecoverAP());
            }
            if (animatorStateInfo.normalizedTime >= 0.9f && (IsAttackingAnimation() || IsNormalAttacking()))
            {
                normalATKCombo = 0;

                if (isSkillAttacking)
                    isSkillAttacking = false;

                if (!player.canMove)
                    player.EnableMove();

                if (isAttacking)
                    isAttacking = false;

                StartCoroutine(CanRecoverAP());
            }

            if (skill1 != null)
            {
                if (skill1.CooldownRate > 0)
                    skill1.CoolingDown(Time.deltaTime);
                imageSkill1.fillAmount = 1 - skill1.CooldownRate;
            }

            if (skill2 != null)
            {
                if (skill2.CooldownRate > 0)
                    skill2.CoolingDown(Time.deltaTime);
                imageSkill2.fillAmount = 1 - skill2.CooldownRate;
            }

            if (skill3 != null)
            {
                if (skill3.CooldownRate > 0)
                    skill3.CoolingDown(Time.deltaTime);
                imageSkill3.fillAmount = 1 - skill3.CooldownRate;
            }

            if (skill4 != null)
            {
                if (skill4.CooldownRate > 0)
                    skill4.CoolingDown(Time.deltaTime);
                imageSkill4.fillAmount = 1 - skill4.CooldownRate;
            }


        }

        public void NormalAttackButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                NormalAttack();
            }
        }

        public void NormalAttack()
        {
            if (!player.IsDashing && canAttack && !isSkillAttacking)
            {
                if (player.IsGrounded)
                {
                    animatorStateInfo = playerAnimation.GetCurrentAnimatorStateInfo();
                    if (normalATKCombo == 0 || IsNormalAttacking() && IsAttackingAnimation() && animatorStateInfo.normalizedTime < 0.8f && normalAttack != null)
                    {
                        if (normalAttack.CanPeform(player.IsGrounded, player) && normalATKCombo < normalAttack.Variations.Count)
                        {
                            normalATKCombo++;
                            normalATKCombo = Mathf.Clamp(normalATKCombo, 1, normalAttack.Variations.Count);
                            currentSkill = normalAttack;
                            currentSkill.SetVariation(normalATKCombo - 1);
                            //SetupBeforeSkill();
                            if (currentSkill.Execute() == true)
                            {
                                isAttacking = true;
                                player.CharacterStats.SetAPRecovery(false);
                                StopCoroutine(CanRecoverAP());
                            }
                        }
                    }
                }
                else
                {
                    if (normalAirAttack != null)
                        if (normalAirAttack.CanPeform(player.IsGrounded, player))
                        {
                            currentSkill = normalAirAttack;
                            //SetupBeforeSkill();
                            if (currentSkill.Execute())
                            {
                                isAttacking = true;
                                player.CharacterStats.SetAPRecovery(false);
                                StopCoroutine(CanRecoverAP());
                            }
                        }
                }
            }
        }

        public void APSet()
        {
            player.CharacterStats.SetAPRecovery(false);
            StartCoroutine(CanRecoverAP());
        }

        private IEnumerator CanRecoverAP()
        {
            yield return new WaitForSeconds(1f);

            if (isAttacking && (IsAttackingAnimation() || IsNormalAttacking()))
            {
                player.CharacterStats.SetAPRecovery(false);
            }
            else
            {
                player.CharacterStats.SetAPRecovery(true);
            }
        }

        protected virtual bool IsAttackingAnimation()
        {
            if (currentSkill != null)
            {
                return animatorStateInfo.IsName(currentSkill.currentVariation.animationName);
            }
            return false;
        }

        protected virtual bool IsNormalAttacking()
        {
            for (int i = 0; i < normalAttack.Variations.Count; i++)
            {
                if (animatorStateInfo.IsName(normalAttack.Variations[i].animationName))
                {
                    return true;
                }
            }
            return false;
        }

        public void SkillButton1(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PerformSkill1();
            }
        }

        public void SkillButton2(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PerformSkill2();
            }
        }

        public void SkillButton3(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PerformSkill3();
            }
        }

        public void SkillButton4(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PerformSkill4();
            }
        }

        public void PerformSkill1()
        {
            if (skill1 != null && !isAttacking && !isSkillAttacking)
            {
                if (skill1.CanPeform(player.IsGrounded, player))
                {
                    currentSkill = skill1;
                    //SetupBeforeSkill();
                    if (currentSkill.Execute())
                    {
                        skill1.EnterCooldown();
                        isAttacking = true;
                        isSkillAttacking = true;
                        player.CharacterStats.SetAPRecovery(false);
                        StopCoroutine(CanRecoverAP());
                    }
                }
            }
        }

        public void PerformSkill2()
        {
            if (skill2 != null && !isAttacking && !isSkillAttacking)
            {
                if (skill2.CanPeform(player.IsGrounded, player))
                {
                    currentSkill = skill2;
                    //SetupBeforeSkill();
                    if (currentSkill.Execute())
                    {
                        skill2.EnterCooldown();
                        isAttacking = true;
                        isSkillAttacking = true;
                        player.CharacterStats.SetAPRecovery(false);
                        StopCoroutine(CanRecoverAP());
                    }
                }
            }
        }

        public void PerformSkill3()
        {
            if (skill3 != null && !isAttacking && !isSkillAttacking)
            {
                if (skill3.CanPeform(player.IsGrounded, player))
                {
                    currentSkill = skill3;
                    //SetupBeforeSkill();
                    if (currentSkill.Execute())
                    {
                        skill3.EnterCooldown();
                        isAttacking = true;
                        isSkillAttacking = true;
                        player.CharacterStats.SetAPRecovery(false);
                        StopCoroutine(CanRecoverAP());
                    }
                }
            }
        }

        public void PerformSkill4()
        {
            if (skill4 != null && !isAttacking && !isSkillAttacking)
            {
                if (skill4.CanPeform(player.IsGrounded, player))
                {
                    currentSkill = skill4;
                    //SetupBeforeSkill();
                    if (currentSkill.Execute())
                    {
                        skill4.EnterCooldown();
                        isAttacking = true;
                        isSkillAttacking = true;
                        player.CharacterStats.SetAPRecovery(false);
                        StopCoroutine(CanRecoverAP());
                    }
                }
            }
        }

        /*protected virtual void SetupBeforeSkill()
        {
            if (currentSkill != null)
            {
                currentSkill.SetupBeforeSkill(player, AttackPoint);
            }
        }*/

        public void SkillDamage()
        {
            if (currentSkill != null)
                currentSkill.Damage();
        }

        public void PerformShot()
        {
            if (currentSkill != null)
                currentSkill.PerformShot();
        }

        private void OnDrawGizmos()
        {
            if (isAttacking)
            {
                currentSkill.DrawGizmo();
            }
        }
    }
}
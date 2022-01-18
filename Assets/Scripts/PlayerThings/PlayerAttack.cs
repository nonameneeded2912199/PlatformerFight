using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PlatformerFight.Abilities;

namespace PlatformerFight.CharacterThings
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Event channels")]
        [SerializeField]
        private InputReader _inputReader = default;

        [SerializeField]
        private SkillEventChannelSO _onSkill1Set = default;

        [SerializeField]
        private SkillEventChannelSO _onSkill2Set = default;

        [SerializeField]
        private SkillEventChannelSO _onSkill3Set = default;

        [SerializeField]
        private SkillEventChannelSO _onSkill4Set = default;

        [Header("Component")]
        protected Rigidbody2D rb;
        protected Player player;
        protected PlayerAnimation playerAnimation;

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
        protected ScriptableSkill normalAirATKScriptable;

        [SerializeField]
        protected Skill normalAirAttack;

        [SerializeField]
        protected ScriptableSkill normalSlideATKScriptable;

        [SerializeField]
        protected Skill normalSlideATK;

        [SerializeField]
        private int normalATKCombo = 0;

        public int Combo => normalATKCombo;

        private Coroutine recoverAPCoroutine;

        private Skill skill1;
        private Skill skill2;
        private Skill skill3;
        private Skill skill4;

        [SerializeField]
        protected List<Skill> listSkill;

        [SerializeField]
        protected List<ScriptableSkill> presetSkill;

        [SerializeField]
        protected Skill currentSkill;

        public delegate void OnSkillBegin();
        public delegate void OnSkillUpdate(float deltaTime);
        public delegate void OnSkillEnd();
        public delegate void OnSkillCancel();

        public OnSkillBegin onSkillBeginAction;
        public OnSkillUpdate onSkillUpdate;
        public OnSkillEnd onSkillEndAction;
        public OnSkillCancel onSkillCancelAction;

        public Transform AttackPoint;

#if UNITY_EDITOR

        [SerializeField]
        private bool debugHitbox;

        [SerializeField]
        private float circleRadius;

        [SerializeField]
        private float horizontalSize;

        [SerializeField]
        private float verticalSize;

#endif

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GetComponent<Player>();
            playerAnimation = GetComponent<PlayerAnimation>();
            
            // Initialize skills
            listSkill = new List<Skill>();

            normalAttack = normalATKScriptable.InitializeSkill(player, AttackPoint);

            normalAirAttack = normalAirATKScriptable.InitializeSkill(player, AttackPoint);

            normalSlideATK = normalSlideATKScriptable.InitializeSkill(player, AttackPoint);

            foreach (ScriptableSkill scriptableSkill in presetSkill)
            {
                listSkill.Add(scriptableSkill.InitializeSkill(player, AttackPoint));
            }

            skill1 = listSkill[0];
            skill2 = listSkill[1];
            skill3 = listSkill[2];
            skill4 = listSkill[3];

            skill1.EnterCooldown();
            skill2.EnterCooldown();
            skill3.EnterCooldown();
            skill4.EnterCooldown();
        }

        private void OnEnable()
        {
            _inputReader.NormalAttackEvent += NormalAttack;
            _inputReader.Skill1Event += PerformSkill1;
            _inputReader.Skill2Event += PerformSkill2;
            _inputReader.Skill3Event += PerformSkill3;
            _inputReader.Skill4Event += PerformSkill4;

            _onSkill1Set.RaiseEvent(skill1);
            _onSkill2Set.RaiseEvent(skill2);
            _onSkill3Set.RaiseEvent(skill3);
            _onSkill4Set.RaiseEvent(skill4);
        }

        private void OnDisable()
        {
            //DiscardCurrentSkill();

            DiscardAllSkill();

            _inputReader.NormalAttackEvent -= NormalAttack;
            _inputReader.Skill1Event -= PerformSkill1;
            _inputReader.Skill2Event -= PerformSkill2;
            _inputReader.Skill3Event -= PerformSkill3;
            _inputReader.Skill4Event -= PerformSkill4;
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            canAttack = true;
        }

        private void Update()
        {
            if (player.IsKnockback && IsAttacking)
            {
                CancelAttack();

                normalATKCombo = 0;

                recoverAPCoroutine = StartCoroutine(CanRecoverAP());
            }
            if (player.IsKnockback)
            {
                canAttack = false;
            }    
            else
            {
                canAttack = true;
            }

            if (skill1 != null)
            {
                if (skill1.CooldownRate > 0)
                    skill1.CoolingDown(Time.deltaTime);
            }

            if (skill2 != null)
            {
                if (skill2.CooldownRate > 0)
                    skill2.CoolingDown(Time.deltaTime);
            }

            if (skill3 != null)
            {
                if (skill3.CooldownRate > 0)
                    skill3.CoolingDown(Time.deltaTime);
            }

            if (skill4 != null)
            {
                if (skill4.CooldownRate > 0)
                    skill4.CoolingDown(Time.deltaTime);
            }

            if (currentSkill == normalSlideATK && !player.wallSlide)
            {
                CancelAttack();
            }    

            if (onSkillUpdate != null)
                onSkillUpdate.Invoke(Time.deltaTime);
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
                    if (normalAttack != null)
                    {
                        if (normalAttack.CanPeform(player.IsGrounded, player) && normalATKCombo < normalAttack.Variations.Count)
                        {
                            normalATKCombo++;
                            normalATKCombo = Mathf.Clamp(normalATKCombo, 1, normalAttack.Variations.Count);
                            normalAttack.SetVariation(normalATKCombo - 1);
                            if (normalAttack.Execute())
                            {
                                SetupSkill(normalAttack);
                                isAttacking = true;
                                player.CharacterStats.SetAPRecovery(false);
                                if (recoverAPCoroutine != null)
                                    StopCoroutine(recoverAPCoroutine);
                                CancelInvoke("ResetNormalAttack");
                                Invoke("ResetNormalAttack", 0.5f);
                            }
                        }
                    }

                }
                else
                {
                    if (player.wallSlide)
                    {
                        if (normalSlideATK != null)
                            if (normalSlideATK.CanPeform(player.IsGrounded, player))
                            {
                                if (normalSlideATK.Execute())
                                {
                                    SetupSkill(normalSlideATK);
                                    isAttacking = true;
                                    player.CharacterStats.SetAPRecovery(false);
                                    if (recoverAPCoroutine != null)
                                        StopCoroutine(recoverAPCoroutine);
                                }
                            }
                    }    
                    else
                    {
                        if (normalAirAttack != null)
                            if (normalAirAttack.CanPeform(player.IsGrounded, player))
                            {
                                if (normalAirAttack.Execute())
                                {
                                    SetupSkill(normalAirAttack);
                                    isAttacking = true;
                                    player.CharacterStats.SetAPRecovery(false);
                                    if (recoverAPCoroutine != null)
                                        StopCoroutine(recoverAPCoroutine);
                                }
                            }
                    }    
                }
            }
        }

        public void APSet()
        {
            player.CharacterStats.SetAPRecovery(false);
            recoverAPCoroutine = StartCoroutine(CanRecoverAP());
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
                foreach (var animationName in currentSkill.currentVariation.animationPlayable)
                {
                    if (player.PlayableDirector.playableAsset == animationName)
                        return true;
                }
            }
            return false;
        }

        protected virtual bool IsNormalAttacking()
        {
            for (int i = 0; i < normalAttack.Variations.Count; i++)
            {
                foreach (var animationName in normalAttack.Variations[i].animationPlayable)
                {
                    if (player.PlayableDirector.playableAsset == animationName)
                        return true;
                }
            }
            return false;
        }

        public void SkillButton1(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PerformSkill(skill1);
            }
        }

        public void SkillButton2(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PerformSkill(skill2);
            }
        }

        public void SkillButton3(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PerformSkill(skill3);
            }
        }

        public void SkillButton4(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PerformSkill(skill4);
            }
        }

        public void PerformSkill(Skill skill)
        {
            if (skill != null && !isAttacking && !isSkillAttacking && canAttack)
            {
                if (skill.CanPeform(player.IsGrounded, player))
                {
                    if (skill.Execute())
                    {
                        skill.EnterCooldown();
                        SetupSkill(skill);
                        isAttacking = true;
                        isSkillAttacking = true;
                        player.CharacterStats.SetAPRecovery(false);
                        if (recoverAPCoroutine != null)
                            StopCoroutine(recoverAPCoroutine);
                    }
                }
            }
        }

        public void PerformSkill1()
        {
            if (skill1 != null && !isAttacking && !isSkillAttacking && canAttack)
            {
                if (skill1.CanPeform(player.IsGrounded, player))
                {
                    if (skill1.Execute())
                    {
                        skill1.EnterCooldown();
                        SetupSkill(skill1);
                        isAttacking = true;
                        isSkillAttacking = true;
                        player.CharacterStats.SetAPRecovery(false);
                        if (recoverAPCoroutine != null)
                            StopCoroutine(recoverAPCoroutine);
                    }
                }
            }
        }

        public void PerformSkill2()
        {
            if (skill2 != null && !isAttacking && !isSkillAttacking && canAttack)
            {
                if (skill2.CanPeform(player.IsGrounded, player))
                {
                    if (skill2.Execute())
                    {
                        skill2.EnterCooldown();
                        SetupSkill(skill2);
                        isAttacking = true;
                        isSkillAttacking = true;
                        player.CharacterStats.SetAPRecovery(false);
                        if (recoverAPCoroutine != null)
                            StopCoroutine(recoverAPCoroutine);
                    }
                }
            }
        }

        public void PerformSkill3()
        {
            if (skill3 != null && !isAttacking && !isSkillAttacking && canAttack)
            {
                if (skill3.CanPeform(player.IsGrounded, player))
                {
                    if (skill3.Execute())
                    {
                        skill3.EnterCooldown();
                        SetupSkill(skill3);
                        isAttacking = true;
                        isSkillAttacking = true;
                        player.CharacterStats.SetAPRecovery(false);
                        if (recoverAPCoroutine != null)
                            StopCoroutine(recoverAPCoroutine);
                    }
                }
            }
        }

        public void PerformSkill4()
        {
            if (skill4 != null && !isAttacking && !isSkillAttacking && canAttack)
            {
                if (skill4.CanPeform(player.IsGrounded, player))
                {
                    if (skill4.Execute())
                    {
                        skill4.EnterCooldown();
                        SetupSkill(skill4);
                        isAttacking = true;
                        isSkillAttacking = true;
                        player.CharacterStats.SetAPRecovery(false);
                        if (recoverAPCoroutine != null)
                            StopCoroutine(recoverAPCoroutine);
                    }
                }
            }
        }

        public void ResetNormalAttack()
        {
            normalATKCombo = 0;
        }

        public void EndAttack()
        {
            isAttacking = false;
            isSkillAttacking = false;
            if (!player.canMove)
            {
                player.EnableMove();
            }

            if (currentSkill != null)
            {
                onSkillEndAction.Invoke();
                DiscardCurrentSkill();
            }

            recoverAPCoroutine = StartCoroutine(CanRecoverAP());
        }    

        public void CancelAttack()
        {
            isAttacking = false;
            isSkillAttacking = false;
            if (!player.canMove)
            {
                player.EnableMove();
            }

            if (currentSkill != null)
            {
                onSkillCancelAction.Invoke();
                DiscardCurrentSkill();
            }

            recoverAPCoroutine = StartCoroutine(CanRecoverAP());
        }   

        protected void SetupSkill(Skill chosenSkill)
        {
            currentSkill = chosenSkill;

            onSkillUpdate += chosenSkill.SkillUpdate;
            onSkillEndAction += chosenSkill.SkillEnd;
            onSkillCancelAction += chosenSkill.SkillCancel;
        }

        protected void DiscardSkill(Skill skill)
        {
            if (skill != null)
            {
                onSkillUpdate -= skill.SkillUpdate;
                onSkillEndAction -= skill.SkillEnd;
                onSkillCancelAction -= skill.SkillCancel;
            }
        }

        protected void DiscardCurrentSkill()
        {
            if (currentSkill != null)
            {
                onSkillUpdate -= currentSkill.SkillUpdate;
                onSkillEndAction -= currentSkill.SkillEnd;
                onSkillCancelAction -= currentSkill.SkillCancel;

                currentSkill = null;
            }           
        }

        protected void DiscardAllSkill()
        {
            DiscardSkill(normalAttack);
            DiscardSkill(normalAirAttack);
            DiscardSkill(skill1);
            DiscardSkill(skill2);
            DiscardSkill(skill3);
            DiscardSkill(skill4);

            currentSkill = null;
        }

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

#if UNITY_EDITOR

            if (debugHitbox)
            {
                Vector2 topLeft = new Vector2(AttackPoint.position.x - horizontalSize / 2, AttackPoint.position.y + verticalSize / 2);

                Vector2 topRight = new Vector2(AttackPoint.position.x + horizontalSize / 2, AttackPoint.position.y + verticalSize / 2);

                Vector2 bottomLeft = new Vector2(AttackPoint.position.x - horizontalSize / 2, AttackPoint.position.y - verticalSize / 2);

                Vector2 bottomRight = new Vector2(AttackPoint.position.x + horizontalSize / 2, AttackPoint.position.y - verticalSize / 2);

                Gizmos.color = Color.magenta;

                Gizmos.DrawLine(topLeft, topRight);
                Gizmos.DrawLine(topLeft, bottomLeft);
                Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.DrawLine(bottomLeft, bottomRight);

                Gizmos.DrawWireSphere(AttackPoint.position, circleRadius);
            }          
#endif
        }
    }
}
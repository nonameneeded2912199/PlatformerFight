using PlatformerFight.Buffs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PlatformerFight.CharacterThings
{
    public abstract class BaseCharacter : MonoBehaviour
    {
        public Rigidbody2D Rigidbody { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }

        public CharacterBuffManager CharacterBuffManager { get; private set; }

        public CharacterStats CharacterStats { get; private set; }
        //public Animator animator { get; private set; }

        [Header("Invincibility Stuff")]
        [SerializeField]
        private Material normalMat;

        [SerializeField]
        private Material whiteMat;

        protected bool IsInvincible { get; private set; }

        public CharacterAnimation CharacterAnimation { get; private set; }

        protected Vector2 velocityWorkspace;

        [Space]
        [Header("Ground Collision Check")]
        [SerializeField]
        private float groundRaycastLength;
        [SerializeField]
        private Vector3 groundRaycastOffset;

        [Header("Wall Collision Check")]
        [SerializeField]
        private float wallRaycastLength;
        public bool onWall;
        public bool onRightWall;

        [Header("Knockback")]
        [SerializeField]
        protected bool canBeKnockedback;

        [SerializeField]
        protected bool knockback;

        public bool IsKnockback { get => knockback; }

        [SerializeField]
        protected Vector2 knockbackSpeed;

        [SerializeField]
        protected float knockbackDuration;

        public float KnockbackDuration { get => knockbackDuration; }

        [Space]
        [Header("Layer")]
        [SerializeField]
        protected LayerMask platformLayer;

        [Space]
        [SerializeField]
        protected TextPopupEventChannelSO popupEventChannel = default;

        [SerializeField]
        protected bool isGrounded;
        public bool IsGrounded { get => isGrounded; }
        public bool facingRight = true;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            CharacterStats = GetComponent<CharacterStats>();
            CharacterAnimation = GetComponent<CharacterAnimation>();
            CharacterBuffManager = GetComponent<CharacterBuffManager>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        protected virtual void FixedUpdate()
        {
            HandleCollision();
        }

        protected virtual void HandleCollision()
        {
            // Ground collision
            isGrounded = Physics2D.Raycast(transform.position + groundRaycastOffset, Vector2.down, groundRaycastLength, platformLayer)
                || Physics2D.Raycast(transform.position - groundRaycastOffset, Vector2.down, groundRaycastLength, platformLayer);

            // Wall collision
            onWall = Physics2D.Raycast(transform.position, Vector2.right, wallRaycastLength, platformLayer)
                || Physics2D.Raycast(transform.position, Vector2.left, wallRaycastLength, platformLayer);

            onRightWall = Physics2D.Raycast(transform.position, Vector2.right, wallRaycastLength, platformLayer);
        }
        public virtual void SetVelocity(float velocity)
        {
            int facingDirection = facingRight ? 1 : -1;
            velocityWorkspace.Set(facingDirection * velocity, Rigidbody.velocity.y);
            Rigidbody.velocity = velocityWorkspace;
        }

        public virtual void SetVelocity(Vector2 velocity, int direction)
        {
            int facingDirection = direction == 1 ? 1 : -1;
            velocityWorkspace.Set(facingDirection * velocity.x, velocity.y);
            Rigidbody.velocity = velocityWorkspace;
        }

        public virtual void SetVelocity(Vector2 velocity)
        {
            int facingDirection = facingRight ? 1 : -1;
            velocityWorkspace.Set(facingDirection * velocity.x, velocity.y);
            Rigidbody.velocity = velocityWorkspace;
        }


        public virtual void SetVelocity(float velocity, Vector2 angle)
        {
            angle.Normalize();
            int facingDirection = facingRight ? 1 : -1;
            velocityWorkspace.Set(angle.x * velocity * facingDirection, angle.y * velocity);
            Rigidbody.velocity = velocityWorkspace;
        }

        public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
            Rigidbody.velocity = velocityWorkspace;
        }

        public virtual void MoveY(float y)
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, y);
        }


        public void SetInvincibility(bool invincibility)
        {
            IsInvincible = invincibility;
        }

        public void Flip()
        {
            facingRight = !facingRight;
            transform.rotation = Quaternion.Euler(0f, facingRight ? 0 : 180f, 0f);
        }

        protected IEnumerator BecomeInvincible(float seconds, bool blinking = true)
        {
            IsInvincible = true;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            for (float i = 0; i < seconds; i += seconds / 10)
            {
                /*if (spriteRenderer.enabled)
                {
                    spriteRenderer.enabled = false;
                }
                else
                {
                    spriteRenderer.enabled = true;
                }*/

                if (spriteRenderer.sharedMaterial == normalMat)
                {
                    spriteRenderer.sharedMaterial = whiteMat;
                }
                else if (spriteRenderer.sharedMaterial == whiteMat)
                {
                    spriteRenderer.sharedMaterial = normalMat;
                }

                yield return new WaitForSeconds(seconds / 10);
            }
            //spriteRenderer.enabled = true;
            spriteRenderer.sharedMaterial = normalMat;
            IsInvincible = false;
        }

        public virtual void Knockback(int direction)
        {
        }

        protected virtual void TakeDamage(AttackDetails attackDetails)
        {

        }

        public void AddBuff(BaseBuff buff)
        {
            CharacterBuffManager.AddBuff(buff);
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position + groundRaycastOffset, transform.position + groundRaycastOffset + Vector3.down * groundRaycastLength);
            Gizmos.DrawLine(transform.position - groundRaycastOffset, transform.position - groundRaycastOffset + Vector3.down * groundRaycastLength);

            // Wall Check
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallRaycastLength);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wallRaycastLength);
        }
    }
}
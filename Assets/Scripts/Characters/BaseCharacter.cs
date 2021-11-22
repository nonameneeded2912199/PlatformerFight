using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterThings
{
    public abstract class BaseCharacter : MonoBehaviour
    {
        public Rigidbody2D Rigidbody { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }

        public CharacterBuffManager characterBuffManager { get; private set; }

        public CharacterStats CharacterStats { get; private set; }
        //public Animator animator { get; private set; }

        [SerializeField]
        protected bool IsInvincible { get; private set; }

        public CharacterAnimation characterAnimation { get; private set; }

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

        [Space]
        [Header("Layer")]
        [SerializeField]
        private LayerMask platformLayer;

        protected bool isGrounded;
        public bool IsGrounded { get => isGrounded; }
        public bool facingRight = true;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            CharacterStats = GetComponent<CharacterStats>();
            characterAnimation = GetComponent<CharacterAnimation>();
            characterBuffManager = GetComponent<CharacterBuffManager>();
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

        public void SetInvincibility(bool invincibility)
        {
            IsInvincible = invincibility;
        }

        public void Flip()
        {
            facingRight = !facingRight;
            transform.rotation = Quaternion.Euler(0f, facingRight ? 0 : 180f, 0f);
        }

        public void AddBuff(string name, BaseBuff buff)
        {
            characterBuffManager.AddBuff(name, buff);
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
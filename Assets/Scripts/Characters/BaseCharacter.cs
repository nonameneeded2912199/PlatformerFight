using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer spriteRender { get; private set; }
    //public Animator animator { get; private set; }

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

    private List<string> outdatedBuff;
    private Dictionary<string, BaseBuff> buffs;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();
        characterAnimation = GetComponent<CharacterAnimation>();
        buffs = new Dictionary<string, BaseBuff>();
    }

    protected virtual void Update()
    {
        outdatedBuff = new List<string>();
        foreach (var buff in buffs)
        {
            var resBuff = buff.Value.Tick(Time.deltaTime);
            if (!resBuff.Item2)
            {
                outdatedBuff.Add(buff.Key);
            }    
        }    

        foreach (var buff in outdatedBuff)
        {
            buffs.Remove(buff);
        }    
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
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle)
    {
        angle.Normalize();
        int facingDirection = facingRight ? 1 : -1;
        velocityWorkspace.Set(angle.x * velocity * facingDirection, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;
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

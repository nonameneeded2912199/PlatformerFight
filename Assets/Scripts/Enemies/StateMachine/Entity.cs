using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Vector2 velocityWorkspace;
    private Vector2 touchDamageBotLeft, touchDamageTopRight;

    private Vector3 initialPosition;

    protected float maxHP;
    public float MaxHP { get => maxHP; }

    [SerializeField]
    protected float currentHP;
    public float CurrentHP { get => currentHP; }
    private float currentStunResistance;
    private float lastDamageTime;

    [SerializeField]
    private bool goRight;

    [SerializeField]
    private float touchDamage, touchDamageWidth, touchDamageHeight;

    [SerializeField]
    private Transform wallPoint;

    [SerializeField]
    private Transform ledgePoint;

    [SerializeField]
    private Transform playerCheckPoint;

    [SerializeField]
    private Transform groundPoint;

    [SerializeField]
    private Transform touchDamagePoint;

    public FiniteStateMachine stateMachine;

    public D_Entity entityData;
    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }

    public AnimationToStateMachine atsm { get; private set; }

    public int facingDirection { get; private set; } = 1;
    public int lastDamageDirection { get; private set; }

    protected bool isStunned;

    protected bool isDead;

    protected bool isInvincible;

    protected AttackDetails touchAttackDetails;

    public virtual void Start()
    {
        initialPosition = transform.position;

        facingDirection = 1;

        switch (GameManager.Instance.currentGameDifficulty)
        {
            case GameDifficulty.EASY:
            case GameDifficulty.NORMAL:
                maxHP = entityData.maxHP;
                break;
            case GameDifficulty.HARD:
                maxHP = entityData.maxHP * 1.5f;
                break;
            case GameDifficulty.LUNATIC:
                maxHP = entityData.maxHP * 2.0f;
                break;
        }

        currentHP = maxHP;

        currentStunResistance = entityData.stunResistance;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        atsm = GetComponent<AnimationToStateMachine>();

        stateMachine = new FiniteStateMachine();

        if (!goRight)
            Flip();
    }

    public virtual void Update()
    {
        stateMachine.CurrentState?.LogicUpdate();

        animator.SetFloat("YVelocity", rb.velocity.y);

        if (Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    public virtual void FixedUpdate()
    {
        CheckTouchDamage();
        stateMachine.CurrentState?.PhysicsUpdate();
    }

    public virtual void LateUpdate()
    {
        stateMachine.CurrentState?.LateUpdate();
    }    

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    public virtual void SetInvincibility(bool invincibility)
    {
        isInvincible = invincibility;
    }    

    public virtual void SetVelocity(float velocity, Vector2 angle)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * facingDirection, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallPoint.position, Vector2.right * facingDirection, entityData.wallRadius,
            entityData.groundLayer);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgePoint.position, Vector2.down, entityData.ledgeRadius,
            entityData.groundLayer);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundPoint.position, entityData.groundRadius, entityData.groundLayer);
    }

    public virtual bool CheckPlayerInMinArgoRange()
    {
        return Physics2D.Raycast(playerCheckPoint.position, transform.right * facingDirection, entityData.minAgroDistance,
            entityData.playerLayer);
    }

    public virtual bool CheckPlayerInMaxArgoRange()
    {
        return Physics2D.Raycast(playerCheckPoint.position, transform.right * facingDirection, entityData.maxAgroDistance,
            entityData.playerLayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheckPoint.position, transform.right * facingDirection, entityData.closeRangeActionDistance,
            entityData.playerLayer);
    }

    public virtual bool CheckPlayerInMinRangedCircle()
    {
        return Physics2D.OverlapCircle(transform.position, entityData.minAgroDistance, entityData.playerLayer);
    }

    public virtual bool CheckPlayerInMaxRangedCircle()
    {
        return Physics2D.OverlapCircle(transform.position, entityData.maxAgroDistance, entityData.playerLayer);
    }

    private void CheckTouchDamage()
    {
        touchDamageBotLeft.Set(touchDamagePoint.position.x - (touchDamageWidth / 2),
                touchDamagePoint.position.y - (touchDamageHeight / 2));
        touchDamageTopRight.Set(touchDamagePoint.position.x + (touchDamageWidth / 2),
            touchDamagePoint.position.y + (touchDamageHeight / 2));

        Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, entityData.playerLayer);

        if (hit != null)
        {
            touchAttackDetails.damageAmount = touchDamage;
            touchAttackDetails.position = transform.position;
            hit.SendMessage("TakeDamage", touchAttackDetails);
        }
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }

    public virtual void TakeDamage(AttackDetails attackDetails)
    {
        if (isInvincible)
            return;

        lastDamageTime = Time.time;

        currentHP -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;

        DamageHop(entityData.damageHopSpeed);

        StartCoroutine(BecomeInvicible(attackDetails.invincibleTime));

        lastDamageDirection = attackDetails.position.x < transform.position.x ? 1 : -1;

        if (currentStunResistance <= 0)
        {
            isStunned = true;
        }

        if (currentHP <= 0)
        {
            isDead = true;
        }
    }

    public virtual void DamageHop(float velocity)
    {
        velocityWorkspace.Set(rb.velocity.x, velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual void Flip()
    {
        facingDirection *= -1;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private IEnumerator BecomeInvicible(float seconds)
    {
        isInvincible = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        for (float i = 0; i < seconds; i += seconds / 10)
        {
            if (spriteRenderer.enabled)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
            }

            yield return new WaitForSeconds(seconds / 10);
        }
        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    public virtual void Respawn()
    {
        transform.position = initialPosition;

        facingDirection = 1;
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x);
        transform.localScale = localScale;

        currentHP = entityData.maxHP;
        isDead = false;
        currentStunResistance = entityData.stunResistance;
        if (!goRight)
            Flip();

        GetComponent<SpriteRenderer>().enabled = true;
        gameObject.SetActive(true);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallPoint.position, wallPoint.position + (Vector3)(Vector2.right * facingDirection * entityData.wallRadius));
        Gizmos.DrawLine(ledgePoint.position, ledgePoint.position + (Vector3)(Vector2.down * entityData.ledgeRadius));
        Gizmos.DrawWireSphere(groundPoint.position, entityData.groundRadius);

        Gizmos.DrawWireSphere(playerCheckPoint.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance * facingDirection), 0.2f);
        Gizmos.DrawWireSphere(playerCheckPoint.position + (Vector3)(Vector2.right * entityData.minAgroDistance * facingDirection), 0.2f);
        Gizmos.DrawWireSphere(playerCheckPoint.position + (Vector3)(Vector2.right * entityData.maxAgroDistance * facingDirection), 0.2f);

        // Detect Circle
        Gizmos.DrawWireSphere(transform.position, entityData.minAgroDistance);
        Gizmos.DrawWireSphere(transform.position, entityData.maxAgroDistance);

        //// touch damage box
        //Vector2 botLeft = new Vector2(touchDamagePoint.position.x - (touchDamageWidth / 2),
        //        touchDamagePoint.position.y - (touchDamageHeight / 2));
        //Vector2 botRight = new Vector2(touchDamagePoint.position.x + (touchDamageWidth / 2),
        //        touchDamagePoint.position.y - (touchDamageHeight / 2));
        //Vector2 topLeft = new Vector2(touchDamagePoint.position.x - (touchDamageWidth / 2),
        //        touchDamagePoint.position.y + (touchDamageHeight / 2));
        //Vector2 topRight = new Vector2(touchDamagePoint.position.x + (touchDamageWidth / 2),
        //        touchDamagePoint.position.y + (touchDamageHeight / 2));

        //Gizmos.DrawLine(botLeft, botRight);
        //Gizmos.DrawLine(botLeft, topLeft);
        //Gizmos.DrawLine(botRight, topRight);
        //Gizmos.DrawLine(topLeft, topRight);

    }
}

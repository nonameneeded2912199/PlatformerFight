using CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : BaseCharacter
{
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
    private Transform ledgePoint;

    [SerializeField]
    private Transform playerCheckPoint;

    [SerializeField]
    private Transform touchDamagePoint;

    public FiniteStateMachine stateMachine;

    public D_Entity entityData;

    public AnimationToStateMachine atsm { get; private set; }

    public int lastDamageDirection { get; private set; }

    protected bool isStunned;

    protected bool isDead;

    protected AttackDetails touchAttackDetails;

    protected override void Start()
    {
        base.Start();
        initialPosition = transform.position;

        facingRight = true;

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

        atsm = GetComponent<AnimationToStateMachine>();

        stateMachine = new FiniteStateMachine();

        if (!goRight)
            Flip();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.CurrentState?.LogicUpdate();

        //animator.SetFloat("YVelocity", rb.velocity.y);

        if (Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        CheckTouchDamage();
        stateMachine.CurrentState?.PhysicsUpdate();
    }

    public virtual void LateUpdate()
    {
        stateMachine.CurrentState?.LateUpdate();
    }    

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgePoint.position, Vector2.down, entityData.ledgeRadius,
            entityData.groundLayer);
    }

    public virtual bool CheckPlayerInMinArgoRange()
    {
        int facingDirection = facingRight ? 1 : -1;
        return Physics2D.Raycast(playerCheckPoint.position, transform.right * facingDirection, entityData.minAgroDistance,
            entityData.playerLayer);
    }

    public virtual bool CheckPlayerInMaxArgoRange()
    {
        int facingDirection = facingRight ? 1 : -1;
        return Physics2D.Raycast(playerCheckPoint.position, transform.right * facingDirection, entityData.maxAgroDistance,
            entityData.playerLayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        int facingDirection = facingRight ? 1 : -1;
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
        if (IsInvincible)
            return;

        lastDamageTime = Time.time;

        currentHP -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;

        GameObject damageOBJ = PoolManager.SpawnObject(GameManager.Instance.DamagePopup);
        DamagePopup damagePopup = damageOBJ.GetComponent<DamagePopup>();
        damagePopup.SetPopup((int)attackDetails.damageAmount, DamageType.NormalDamage, transform.position);

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
        velocityWorkspace.Set(Rigidbody.velocity.x, velocity);
        Rigidbody.velocity = velocityWorkspace;
    }

    private IEnumerator BecomeInvicible(float seconds)
    {
        SetInvincibility(true);
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
        SetInvincibility(false);
    }

    public virtual void Respawn()
    {
        transform.position = initialPosition;

        facingRight = true;
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
        SetInvincibility(false);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        int facingDirection = facingRight ? 1 : -1;

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

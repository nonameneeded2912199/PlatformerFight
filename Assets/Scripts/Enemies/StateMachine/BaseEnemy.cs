using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseCharacter
{
    private Vector2 touchDamageBotLeft, touchDamageTopRight;


    private float currentStunResistance;

    private float currentKnockbackResistance;

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

    [SerializeField]
    protected bool canDestroy;

    [SerializeField]
    protected BulletEventChannelSO bulletEventChannel;

    public BulletEventChannelSO BulletEventChannel => bulletEventChannel;

    [SerializeField]
    protected GameStateSO gameStateSO;

    public GameStateSO GameStateSO => gameStateSO;

    public FiniteStateMachine stateMachine;

    public State reservedState { get; set; }

    public D_Enemy entityData;

    public AnimationToStateMachine atsm { get; private set; }

    public int lastDamageDirection { get; private set; }

    protected bool isStunned;

    protected bool isDead;

    protected AttackDetails touchAttackDetails;

    protected override void Awake()
    {
        base.Awake();
        atsm = GetComponent<AnimationToStateMachine>();

        stateMachine = new FiniteStateMachine();
    }

    protected override void Start()
    {
        base.Start();

        facingRight = true;

        currentStunResistance = entityData.stunResistance;
        currentKnockbackResistance = entityData.knockbackResistance;

        if (!goRight)
            Flip();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.CurrentState?.LogicUpdate();

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
            platformLayer);
    }

    public virtual bool CheckPlayerInMinArgoRange()
    {
        int facingDirection = facingRight ? 1 : -1;
        return Physics2D.Raycast(playerCheckPoint.position, Vector3.right * facingDirection, entityData.minAgroDistance,
            entityData.playerLayer);
    }

    public virtual bool CheckPlayerInMaxArgoRange()
    {
        int facingDirection = facingRight ? 1 : -1;
        return Physics2D.Raycast(playerCheckPoint.position, Vector3.right * facingDirection, entityData.maxAgroDistance,
            entityData.playerLayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        int facingDirection = facingRight ? 1 : -1;
        return Physics2D.Raycast(playerCheckPoint.position, Vector3.right * facingDirection, entityData.closeRangeActionDistance,
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
            touchAttackDetails.damageAmount = (int)(CharacterStats.CurrentAttack * 0.75f);
            touchAttackDetails.position = transform.position;
            hit.SendMessage("TakeDamage", touchAttackDetails);
        }
    }

    public virtual void ResetKnockbackResistance()
    {
        currentKnockbackResistance = entityData.knockbackResistance;
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }

    protected override void TakeDamage(AttackDetails attackDetails)
    {
        if (IsInvincible || stateMachine.CurrentState is KnockbackState)
            return;

        lastDamageTime = Time.time;

        float reduction = CharacterStats.CurrentDefense / (CharacterStats.CurrentDefense + 500);
        float multiplier = 1 - reduction;
        int incomingDMG = (int)(attackDetails.damageAmount * multiplier);

        CharacterStats.CurrentHP -= incomingDMG;

        if (canBeKnockedback)
            currentKnockbackResistance--;
        currentStunResistance -= attackDetails.stunDamageAmount;

        //GameObject damageOBJ = PoolManager.SpawnObject(DamagePopup.OriginalDamagePopup);
        //DamagePopup damagePopup = damageOBJ.GetComponent<DamagePopup>();
        //damagePopup.SetPopup(incomingDMG, DamageType.NormalDamage, transform.position);

        popupEventChannel.RaiseTextPopupEvent(incomingDMG.ToString(), transform.position);

        StartCoroutine(BecomeInvincible(attackDetails.invincibleTime));

        lastDamageDirection = attackDetails.position.x < transform.position.x ? 1 : -1;

        if (currentStunResistance <= 0)
        {
            isStunned = true;
        }

        if (CharacterStats.CurrentHP <= 0)
        {
            isDead = true;
        }

        if (currentKnockbackResistance <= 0 && canBeKnockedback)
        {
            reservedState = stateMachine.CurrentState;
            stateMachine.ChangeState(new KnockbackState(stateMachine, this, "Hurt"));           
        }
    }

    public override void Knockback(int direction)
    {
        Rigidbody.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    protected virtual void OnBecameInvisible()
    {
        if (canDestroy)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        int facingDirection = facingRight ? 1 : -1;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(playerCheckPoint.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance * facingDirection), 0.2f);
        Gizmos.DrawWireSphere(playerCheckPoint.position + (Vector3)(Vector2.right * entityData.minAgroDistance * facingDirection), 0.2f);
        Gizmos.DrawWireSphere(playerCheckPoint.position + (Vector3)(Vector2.right * entityData.maxAgroDistance * facingDirection), 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(ledgePoint.position, ledgePoint.position + Vector3.down * entityData.ledgeRadius);

        Gizmos.color = Color.green;
        // Detect Circle
        //Gizmos.DrawWireSphere(transform.position, entityData.minAgroDistance);
        //Gizmos.DrawWireSphere(transform.position, entityData.maxAgroDistance);


        Gizmos.color = Color.blue;
        // touch damage box
        Vector2 botLeft = new Vector2(touchDamagePoint.position.x - (touchDamageWidth / 2),
                touchDamagePoint.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamagePoint.position.x + (touchDamageWidth / 2),
                touchDamagePoint.position.y - (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamagePoint.position.x - (touchDamageWidth / 2),
                touchDamagePoint.position.y + (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamagePoint.position.x + (touchDamageWidth / 2),
                touchDamagePoint.position.y + (touchDamageHeight / 2));

        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botLeft, topLeft);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topLeft, topRight);

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Bullet : MonoBehaviour
{
    private AttackDetails attackDetails;

    private List<BulletCommand> bulletCommands;

    private float suspendTime = 0f;

    [SerializeField]
    private BulletEventChannelSO bulletEventChannel = default;

    public SpriteRenderer spriteRenderer { get; private set; }

    public Animator animator { get; private set; }

    public float Speed { get; set; }

    public float Direction { get; set; }

    public float Acceleration { get; set; }

    public float LifeSpan { get; set; }

    public bool Pierce { get; set; }

    public bool HasLifeSpan { get; set; }

    public bool destroyOnInvisible { get; set; } = true;

    public bool walkThroughPlatform { get; set; } = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        bulletCommands = new List<BulletCommand>();
        Physics2D.reuseCollisionCallbacks = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
            return;
        if (suspendTime <= 0)
        {
            Speed += Acceleration;
            //Direction += Curve;

            if (Speed < 0)
                Speed = 0;

            Vector3 bulletPos = transform.position;
            bulletPos.x += Speed * Mathf.Cos(Direction) * 1 / 60f;
            bulletPos.y += Speed * Mathf.Sin(Direction) * 1 / 60f;
            transform.rotation = Quaternion.Euler(0, 0, Direction * Mathf.Rad2Deg);
            transform.position = bulletPos;

            UpdateBulletCommands();
        }
        else
        {
            suspendTime -= Time.deltaTime;
            return;
        }

        if (HasLifeSpan)
        {
            LifeSpan -= Time.deltaTime;
            if (LifeSpan <= 0)
            {
                BackToPool();
            }
        }
    }

    public void UpdateBulletCommands()
    {
        for (int i = 0; i < bulletCommands.Count; i++)
        {
            BulletCommand bc = bulletCommands[i];
            if (bc.IsEnoughTime)
            {
                bulletCommands.Remove(bc);
                i--;
            }
            else
            {
                bc.Update();
                if (bc.IsExecutable)
                {
                    bc.Execute(this);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (!walkThroughPlatform)
            {
                BackToPool();
                return;
            }

        }

        attackDetails.position = transform.position;

        if (!collision.CompareTag("Player") && !collision.CompareTag("Enemy"))
            return;

        if (!gameObject.CompareTag(collision.tag))
        {
            if (collision.gameObject.GetComponent<PlatformerFight.CharacterThings.BaseCharacter>().IsDead)
                return;

            if (collision.gameObject.layer == LayerMask.NameToLayer("Shield"))
            {
                BackToPool();
                return;
            }
        }

        //attackDetails.stunDamageAmount = 0;
        switch (gameObject.tag)
        {
            case "Player":
                if (collision.CompareTag("Enemy"))
                {
                    collision.SendMessage("TakeDamage", attackDetails);
                    if (!Pierce)
                        BackToPool();
                }
                break;

            case "Enemy":
                if (collision.CompareTag("Player"))
                {
                    collision.SendMessage("TakeDamage", attackDetails);
                    if (!Pierce)
                        BackToPool();
                }
                break;
        }
    }

    private void OnBecameInvisible()
    {
        if (destroyOnInvisible)
            BackToPool();
    }

    private void BackToPool()
    {
        if (enabled)
        {
            HasLifeSpan = false;

            bulletCommands.Clear();

            ResetAttributes();

        }

        bulletEventChannel.RaiseReturnBulletEvent(this);
    }

    public void EndBullet()
    {
        BackToPool();
    }

    public void ChangeSprite(Sprite sprite, AnimatorOverrideController animatorOverrideController)
    {
        animator.runtimeAnimatorController = animatorOverrideController;
        spriteRenderer.sprite = sprite;
        Collider2D thisCollider = gameObject.GetComponent<CircleCollider2D>();
        Destroy(thisCollider);
        thisCollider = gameObject.AddComponent<CircleCollider2D>();
        thisCollider.isTrigger = true;
    }

    public void SetAllegiance(string owner)
    {
        gameObject.tag = owner;
    }

    public void SetAttributes(Vector2 position, float speed, float direction, float acceleration, float lifeSpan, float damage, float invincibleTime, 
        float suspendTime, bool pierce = false, bool destroyOnInvisible = true, bool walkThroughPlatform = false)
    {
        ResetAttributes();

        transform.position = position;
        Speed = speed;
        Direction = direction;
        Acceleration = acceleration;
        Pierce = pierce;

        if (lifeSpan > 0)
        {
            HasLifeSpan = true;
            this.LifeSpan = lifeSpan;
        }
        else
        {
            HasLifeSpan = false;
        }

        this.suspendTime = suspendTime;

        attackDetails.damageAmount = damage;
        attackDetails.invincibleTime = invincibleTime;
        this.destroyOnInvisible = destroyOnInvisible;
        this.walkThroughPlatform = walkThroughPlatform;
    }

    public void ResetAttributes()
    {
        transform.position = Vector3.zero;
        Direction = 0;
        Speed = 0;
        Acceleration = 0;
        HasLifeSpan = false;
        LifeSpan = 0;
        destroyOnInvisible = true;
        walkThroughPlatform = false;
    }

    public void AddBulletCommand(Action<Bullet> command, int durationFrames, int executeLimit = 1, int startOffset = 0)
    {
        BulletCommand newCommand = new BulletCommand(command, durationFrames, executeLimit, startOffset);
        bulletCommands.Add(newCommand);
    }    

    public void SetDisappear(int frame)
    {
        Action<Bullet> disappear = new Action<Bullet>((bullet) =>
        {
            bullet.EndBullet();
        });
        BulletCommand disappearCommand = new BulletCommand(disappear, frame);
        bulletCommands.Add(disappearCommand);
    }    
}
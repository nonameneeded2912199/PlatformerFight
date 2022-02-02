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

    public CircleCollider2D bulletCollider { get; private set; }

    public SpriteRenderer spriteRenderer { get; private set; }

    public Animator animator { get; private set; }

    public float Speed { get; set; }

    public float Direction { get; set; }

    public float Acceleration { get; set; }

    public float LifeSpan { get; set; }

    public bool Pierce { get; set; }

    public bool hasLifeSpan { get; set; }

    public bool destroyOnInvisible { get; set; } = true;

    public bool walkThroughWall { get; set; } = true;

    private void Awake()
    {
        bulletCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        bulletCommands = new List<BulletCommand>();
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

            Vector3 bulletPos = transform.position;
            bulletPos.x += Speed * Mathf.Cos(Direction) * Time.deltaTime;
            bulletPos.y += Speed * Mathf.Sin(Direction) * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, Direction * Mathf.Rad2Deg);
            transform.position = bulletPos;

            UpdateBulletCommands();
        }
        else
        {
            suspendTime -= Time.deltaTime;
        }

        if (hasLifeSpan)
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
        if (!walkThroughWall)
        {
            int platformLayer = 0;
            platformLayer |= (1 << LayerMask.NameToLayer("Ground"));
            if (collision.gameObject.layer == platformLayer)
            {
                BackToPool();
                return;
            }
        }

        attackDetails.position = transform.position;

        int damagableLayer = 0;
        damagableLayer |= (1 << LayerMask.NameToLayer("Shield"));
        damagableLayer |= (1 << LayerMask.NameToLayer("Damagable"));

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
            hasLifeSpan = false;
            /*BulletCommand[] bulletCommands = gameObject.GetComponents<BulletCommand>();
            foreach (BulletCommand bc in bulletCommands)
            {
                Destroy(bc);
            }*/

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
        animator.runtimeAnimatorController = animatorOverrideController as RuntimeAnimatorController;
        spriteRenderer.sprite = sprite;
    }

    public void ChangeHitRadius(float radius)
    {
        bulletCollider.radius = radius;
    }

    public void SetAllegiance(string owner)
    {
        gameObject.tag = owner;
    }

    public void SetAttributes(Vector2 position, float speed, float direction, float acceleration, float lifeSpan, 
        float damage, float invincibleTime, float suspendTime, bool pierce = false, bool destroyOnInvisible = true, bool walkThroughWall = true)
    {
        ResetAttributes();

        transform.position = position;
        Speed = speed;
        Direction = direction;
        Acceleration = acceleration;
        Pierce = pierce;

        if (lifeSpan > 0)
        {
            hasLifeSpan = true;
            this.LifeSpan = lifeSpan;
        }
        else
        {
            hasLifeSpan = false;
        }

        this.suspendTime = suspendTime;

        attackDetails.damageAmount = damage;
        attackDetails.invincibleTime = invincibleTime;
        this.destroyOnInvisible = destroyOnInvisible;
        this.walkThroughWall = walkThroughWall;
    }

    public void ResetAttributes()
    {
        transform.position = Vector3.zero;
        Direction = 0;
        Speed = 0;
        Acceleration = 0;
        hasLifeSpan = false;
        LifeSpan = 0;
        destroyOnInvisible = true;
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
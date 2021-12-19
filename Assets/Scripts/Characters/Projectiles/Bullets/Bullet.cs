using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Bullet : MonoBehaviour
{
    private float lifeTime;
    private AttackDetails attackDetails;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private BulletEventChannelSO bulletEventChannel = default;

    public CircleCollider2D bulletCollider { get; private set; }

    public SpriteRenderer spriteRenderer { get; private set; }

    public Animator animator { get; private set; }

    public float Speed { get; set; }

    public float Direction { get; set; }

    public float Acceleration { get; set; }

    public float LifeSpan { get; set; }

    public bool hasLifeSpan { get; set; }

    public bool destroyOnInvisible { get; set; } = true;

    public bool IsDelayed { get; set; } = false;

    private void Awake()
    {
        bulletCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //attackDetails.position = transform.position;

        Speed += Acceleration;
        //Direction += Curve;

        Vector3 bulletPos = transform.position;
        bulletPos.x += Speed * Mathf.Cos(Direction) * Time.deltaTime;
        bulletPos.y += Speed * Mathf.Sin(Direction) * Time.deltaTime;      
        transform.rotation = Quaternion.Euler(0, 0, Direction * Mathf.Rad2Deg);
        transform.position = bulletPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        attackDetails.position = transform.position;

        int damagableLayer = 0;
        damagableLayer |= (1 << LayerMask.NameToLayer("Shield"));
        damagableLayer |= (1 << LayerMask.NameToLayer("Damagable"));

        if (collision.gameObject.layer == LayerMask.NameToLayer("Shield") && !gameObject.CompareTag(collision.tag))
        {
            BackToPool();
            return;
        }

        //attackDetails.stunDamageAmount = 0;
        switch (gameObject.tag)
        {
            case "Player":
                if (collision.CompareTag("Enemy"))
                {
                    collision.SendMessage("TakeDamage", attackDetails);
                }
                break;

            case "Enemy":
                if (collision.CompareTag("Player"))
                {
                    collision.SendMessage("TakeDamage", attackDetails);
                }
                break;
        }
    }

    private void LateUpdate()
    {
        if (hasLifeSpan && !IsDelayed)
        {
            lifeTime += Time.deltaTime;
            if (lifeTime >= LifeSpan)
            {
                lifeTime = 0;
                BackToPool();
            }    
        }    
    }

    private void OnBecameInvisible()
    {
        if (IsDelayed)
            return;

        if (destroyOnInvisible)
            BackToPool();
    }

    private void BackToPool()
    {
        if (enabled)
        {
            hasLifeSpan = false;
            BulletCommand[] bulletCommands = gameObject.GetComponents<BulletCommand>();
            foreach (BulletCommand bc in bulletCommands)
            {
                Destroy(bc);
            }

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

    public void ExitDelay()
    {
        gameObject.SetActive(true);
        IsDelayed = false;
    }
    
    public void SetAllegiance(string owner)
    {
        gameObject.tag = owner;
    }

    public void SetAttributes(Vector2 position, float speed, float direction, float acceleration, float lifeSpan, float damage, float invincibleTime, 
        float delay = 0, bool destroyOnInvisible = true)
    {
        ResetAttributes();

        transform.position = position;
        Speed = speed;
        Direction = direction;
        Acceleration = acceleration;

        if (delay > 0)
        {
            IsDelayed = true;
            gameObject.SetActive(false);
            Invoke("ExitDelay", delay);
        }

        if (lifeSpan > 0)
        {
            LifeSpan = lifeSpan;
            hasLifeSpan = true;
        }
        else
        {
            hasLifeSpan = false;
        }

        attackDetails.damageAmount = damage;
        attackDetails.invincibleTime = invincibleTime;
        this.destroyOnInvisible = destroyOnInvisible;
    }
    
    public void ResetAttributes()
    {
        transform.position = Vector3.zero;
        Direction = 0;
        Speed = 0;
        Acceleration = 0;
        hasLifeSpan = false;
        LifeSpan = 0;
        lifeTime = 0;
        destroyOnInvisible = true;

    }
}

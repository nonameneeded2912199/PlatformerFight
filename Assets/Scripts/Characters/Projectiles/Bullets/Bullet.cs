using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        bulletCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /*public static GameObject GetBullet(BulletOwner owner, Vector2 position, float speed, float direction, float lifeSpan, float damage, 
        BulletType bulletType, BulletColor bulletColor, bool destroyOnInvisible = true)
    {
        GameObject bulletObj = PoolManager.Instance.GetPoolObject(PoolObjectType.Bullet);
        switch (owner)
        {
            case BulletOwner.Player:
                bulletObj.tag = "Player";
                break;
            case BulletOwner.Enemy:
            default:
                bulletObj.tag = "Enemy";
                break;
        }
        bulletObj.transform.position = position;
        Bullet bulletCom = bulletObj.GetComponent<Bullet>();
        bulletCom.Speed = speed;
        bulletCom.Direction = direction;

        if (lifeSpan > 0)
        {
            bulletCom.LifeSpan = lifeSpan;
            bulletCom.hasLifeSpan = true;
        }

        bulletCom.ChangeSprite(bulletType, bulletColor);

        bulletCom.attackDetails.damageAmount = damage;
        bulletObj.SetActive(true);
        bulletCom.destroyOnInvisible = destroyOnInvisible;
        return bulletObj;
    }

    public static GameObject GetBullet(BulletOwner owner, Vector2 position, float speed, float direction, float acceleration, float lifeSpan, float damage,
        BulletType bulletType, BulletColor bulletColor, bool destroyOnInvisible = true)
    {
        GameObject bulletObj = PoolManager.Instance.GetPoolObject(PoolObjectType.Bullet);
        switch (owner)
        {
            case BulletOwner.Player:
                bulletObj.tag = "Player";
                break;
            case BulletOwner.Enemy:
            default:
                bulletObj.tag = "Enemy";
                break;
        }
        bulletObj.transform.position = position;
        Bullet bulletCom = bulletObj.GetComponent<Bullet>();
        bulletCom.Speed = speed;
        bulletCom.Direction = direction;
        bulletCom.Acceleration = acceleration;

        if (lifeSpan > 0)
        {
            bulletCom.LifeSpan = lifeSpan;
            bulletCom.hasLifeSpan = true;
        }

        bulletCom.ChangeSprite(bulletType, bulletColor);

        bulletCom.attackDetails.damageAmount = damage;
        bulletObj.SetActive(true);
        bulletCom.destroyOnInvisible = destroyOnInvisible;
        return bulletObj;
    }*/

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
        if (hasLifeSpan)
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
        if (destroyOnInvisible)
            BackToPool();
    }

    //private void OnDisable()
    //{
    //    if (enabled)
    //    {
    //        hasLifeSpan = false;
    //        BulletCommand[] bulletCommands = gameObject.GetComponents<BulletCommand>();
    //        foreach (BulletCommand bc in bulletCommands)
    //        {
    //            Destroy(bc);
    //        }

    //        ResetAttributes();

    //    }    
    //}

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
        //bool returnToPool = PoolManager.ReleaseObject(gameObject);
    }    

    public static float GetAngle(Vector3 o, Vector3 vector)
    {
        float angle;
        vector -= o;
        Vector3 cross = Vector3.Cross(Vector3.right, vector);
        angle = Vector2.Angle(Vector3.right, vector) * Mathf.Deg2Rad;
        return cross.z > 0 ? angle : -angle;
    }

    public static float GetAngle(Vector3 vector)
    {
        float angle;
        Vector3 cross = Vector3.Cross(Vector3.right, vector);
        angle = Vector2.Angle(Vector3.right, vector) * Mathf.Deg2Rad;
        return cross.z > 0 ? angle : -angle;
    }

    public Vector2 GetSpeedVector()
    {
        return new Vector2(Speed * Mathf.Cos(Direction), Speed * Mathf.Sin(Direction));
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

    public void ChangeSprite(BulletType bulletType, BulletColor bulletColor)
    {
        //switch (bulletType)
        //{
        //    case BulletType.Arrow:
        //    case BulletType.Ball:
        //    case BulletType.Ball2:
        //        damageRadius = 0.1f;
        //        hitBox.localPosition = Vector2.zero;
        //        break;
        //    case BulletType.Bullet:
        //        damageRadius = 0.08f;
        //        hitBox.localPosition = Vector2.zero;
        //        break;
        //    case BulletType.Ice:
        //        damageRadius = 0.06f;
        //        hitBox.localPosition = Vector2.zero;
        //        break;
        //    case BulletType.Inverted:
        //        damageRadius = 0.07f; 
        //        hitBox.localPosition = Vector2.zero;
        //        break;
        //    case BulletType.Kunai:
        //        damageRadius = 0.07f;
        //        hitBox.localPosition = new Vector2(0.03f, 0);
        //        break;
        //    case BulletType.Rice:
        //        damageRadius = 0.07f;
        //        hitBox.localPosition = Vector2.zero;
        //        break;
        //    case BulletType.Square:
        //        damageRadius = 0.1f;
        //        hitBox.localPosition = Vector2.zero;
        //        break;
        //    case BulletType.Star:
        //        damageRadius = 0.07f;
        //        hitBox.localPosition = Vector2.zero;
        //        break;
        //}

        //GetComponent<SpriteRenderer>().sprite = BulletGraphicLoader.Instance.GetBulletGraphics(bulletType, bulletColor);
    }   
    
    public void SetAllegiance(string owner)
    {
        gameObject.tag = owner;
    }

    public void SetAttributes(Vector2 position, float speed, float direction, float acceleration, float lifeSpan, float damage, float invincibleTime, bool destroyOnInvisible = true)
    {
        ResetAttributes();

        transform.position = position;
        Speed = speed;
        Direction = direction;
        Acceleration = acceleration;

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

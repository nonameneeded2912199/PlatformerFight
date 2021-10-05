using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifeTime;
    private AttackDetails attackDetails;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private float damageRadius;

    [SerializeField]
    private Transform hitBox;

    public float Speed { get; set; }

    public float Direction { get; set; }

    public float Acceleration { get; set; }

    public float LifeSpan { get; set; }

    public bool hasLifeSpan { get; set; }

    public bool destroyOnInvisible { get; set; } = true;

    public CharacterController player { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static GameObject GetBullet(Vector2 position, float speed, float direction, float lifeSpan, float damage, 
        BulletType bulletType, BulletColor bulletColor, bool destroyOnInvisible = true)
    {
        GameObject bulletObj = PoolManager.Instance.GetPoolObject(PoolObjectType.Bullet);
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

    public static GameObject GetBullet(Vector2 position, float speed, float direction, float acceleration, float lifeSpan, float damage,
        BulletType bulletType, BulletColor bulletColor, bool destroyOnInvisible = true)
    {
        GameObject bulletObj = PoolManager.Instance.GetPoolObject(PoolObjectType.Bullet);
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
    }    

    // Update is called once per frame
    void Update()
    {
        attackDetails.position = transform.position;

        Speed += Acceleration;
        //Direction += Curve;

        Vector3 bulletPos = transform.position;
        bulletPos.x += Speed * Mathf.Cos(Direction) * Time.deltaTime;
        bulletPos.y += Speed * Mathf.Sin(Direction) * Time.deltaTime;      
        transform.rotation = Quaternion.Euler(0, 0, Direction * Mathf.Rad2Deg);
        transform.position = bulletPos;
    }

    private void FixedUpdate()
    {
        Collider2D damageHit = Physics2D.OverlapCircle(hitBox.position, damageRadius, playerLayer);

        if (damageHit)
        {
            damageHit.transform.SendMessage("TakeDamage", attackDetails);
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
                gameObject.SetActive(false);
            }    
        }    
    }

    private void OnBecameInvisible()
    {
        if (destroyOnInvisible)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        player = GameObject.Find("Player").GetComponent<CharacterController>();
    }

    private void OnDisable()
    {
        hasLifeSpan = false;
        BulletCommand[] bulletCommands = gameObject.GetComponents<BulletCommand>();
        foreach (BulletCommand bc in bulletCommands)
        {
            Destroy(bc);
        }

        ResetAttributes();

        if (PoolManager.Instance != null)
            PoolManager.Instance.CoolObject(this.gameObject, PoolObjectType.Bullet);
        else
            Destroy(gameObject);
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

    public void ChangeSprite(BulletType bulletType, BulletColor bulletColor)
    {
        switch (bulletType)
        {
            case BulletType.Arrow:
            case BulletType.Ball:
            case BulletType.Ball2:
                damageRadius = 0.1f;
                hitBox.localPosition = Vector2.zero;
                break;
            case BulletType.Bullet:
                damageRadius = 0.08f;
                hitBox.localPosition = Vector2.zero;
                break;
            case BulletType.Ice:
                damageRadius = 0.06f;
                hitBox.localPosition = Vector2.zero;
                break;
            case BulletType.Inverted:
                damageRadius = 0.07f; 
                hitBox.localPosition = Vector2.zero;
                break;
            case BulletType.Kunai:
                damageRadius = 0.07f;
                hitBox.localPosition = new Vector2(0.03f, 0);
                break;
            case BulletType.Rice:
                damageRadius = 0.07f;
                hitBox.localPosition = Vector2.zero;
                break;
            case BulletType.Square:
                damageRadius = 0.1f;
                hitBox.localPosition = Vector2.zero;
                break;
            case BulletType.Star:
                damageRadius = 0.07f;
                hitBox.localPosition = Vector2.zero;
                break;
        }

        GetComponent<SpriteRenderer>().sprite = BulletGraphicLoader.Instance.GetBulletGraphics(bulletType, bulletColor);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hitBox.position, damageRadius);
    }
}

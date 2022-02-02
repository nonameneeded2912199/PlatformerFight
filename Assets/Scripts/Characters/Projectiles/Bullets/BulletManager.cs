using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [Header("Bullets pool")]
    [SerializeField]
    private BulletPoolSO bulletPool = default;
    [SerializeField]
    private int initialSize = 10;

    [Header("Listening on channels")]
    [Tooltip("The BulletManager listens to this event, fired by objects in any scene, to spawn Bullet")]
    [SerializeField]
    private BulletEventChannelSO bulletEventChannel = default;

    private void Awake()
    {
        bulletPool.Prewarm(initialSize);
        bulletPool.SetParent(transform);
    }

    private void OnEnable()
    {
        bulletEventChannel.OnBulletSpawnRequested += SpawnBullet;
        bulletEventChannel.OnBulletBackToPoolRequested += RetrieveBullet;
    }

    private void OnDestroy()
    {
        bulletEventChannel.OnBulletSpawnRequested -= SpawnBullet;
        bulletEventChannel.OnBulletBackToPoolRequested -= RetrieveBullet;

        bulletPool.RequestResetPool();
    }

    private Bullet SpawnBullet(string allegiance, Vector2 position, float speed, float direction, float acceleration, float lifeSpan, float damage,
        float invincibleTime, float radius, Sprite sprite, AnimatorOverrideController animatorOverrideController, 
        float suspendTime = 0, bool pierce = false, bool destroyOnInvisible = true, bool walkThroughWall = true)
    {
        Bullet bullet = bulletPool.Request();
        bullet.SetAllegiance(allegiance);
        bullet.ChangeSprite(sprite, animatorOverrideController);
        bullet.ChangeHitRadius(radius);
        bullet.SetAttributes(position, speed, direction, acceleration, lifeSpan, damage, invincibleTime, suspendTime, pierce, destroyOnInvisible, walkThroughWall);

        return bullet;
    }

    private bool RetrieveBullet(Bullet bullet)
    {
        try
        {
            bulletPool.Return(bullet);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        return false;
    }
}
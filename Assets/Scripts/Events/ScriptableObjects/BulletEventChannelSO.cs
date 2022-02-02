using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Bullet Event Channel")]
public class BulletEventChannelSO : DescriptionBaseSO
{
    public delegate Bullet BulletSpawnAction(string allegiance, Vector2 position, float speed, float direction, float acceleration, float lifeSpan,
        float damage, float invincibleTime, float radius, Sprite sprite, AnimatorOverrideController animatorOverrideController, 
        float delay = 0, bool pierce = false, bool destroyOnInvisible = true, bool walkThroughWall = true);

    public delegate bool BulletBackToPoolAction(Bullet bullet);

    public BulletSpawnAction OnBulletSpawnRequested;
    public BulletBackToPoolAction OnBulletBackToPoolRequested;

    public Bullet RaiseBulletEvent(string allegiance, Vector2 position, float speed, float direction, float acceleration, float lifeSpan,
        float damage, float invincibleTime, float radius, Sprite sprite, AnimatorOverrideController animatorOverrideController, 
        float suspendTime = 0, bool pierce = false, bool destroyOnInvisible = true, bool walkThroughWall = true)
    {
        Bullet bullet = default;

        if (OnBulletSpawnRequested != null)
        {
            bullet = OnBulletSpawnRequested.Invoke(allegiance, position, speed, direction, acceleration, lifeSpan, damage, invincibleTime, radius, sprite,
                animatorOverrideController, suspendTime, pierce, destroyOnInvisible, walkThroughWall);
        }
        else
        {
            Debug.LogWarning("An Bullet play event was requested  for " + bullet.name + ", but nobody picked it up. " +
                "Check why there is no BulletManager already loaded, " +
                "and make sure it's listening on this Bullet Event channel.");
        }

        return bullet;
    }

    public bool RaiseReturnBulletEvent(Bullet bullet)
    {
        bool requestSucceed = false;

        if (OnBulletBackToPoolRequested != null)
        {
            requestSucceed = OnBulletBackToPoolRequested.Invoke(bullet);
        }
        else
        {
            Debug.LogWarning("An Bullet play event was requested  for " + bullet.name + ", but nobody picked it up. " +
                "Check why there is no BulletManager already loaded, " +
                "and make sure it's listening on this Bullet Event channel.");
        }

        return requestSucceed;
    }
}
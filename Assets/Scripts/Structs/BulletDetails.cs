using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BulletDetails
{
    public float bulletSpeed;
    public float bulletAcceleration;
    public float bulletLifeSpan;
    public float damageMultiplier;

    public bool destroyOnInvisible;

    public Sprite bulletSprite;
    public AnimatorOverrideController animatorOverrideController;    
}

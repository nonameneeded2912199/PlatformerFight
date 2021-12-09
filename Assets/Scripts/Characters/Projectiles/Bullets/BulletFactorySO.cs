using PlatformerFight.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBulletFactory", menuName = "Factory/Bullet Factory")]
public class BulletFactorySO : FactorySO<Bullet>
{
    public Bullet bulletPrefab = default;

    public override Bullet Create()
    {
        return Instantiate(bulletPrefab);
    }
}

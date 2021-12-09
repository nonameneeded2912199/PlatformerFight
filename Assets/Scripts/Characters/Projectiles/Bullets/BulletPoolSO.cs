using PlatformerFight.Factory;
using PlatformerFight.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBulletPool", menuName = "Pool/Bullet Pool")]
public class BulletPoolSO : ComponentPoolSO<Bullet>
{
	[SerializeField] private BulletFactorySO _factory;

	public override IFactory<Bullet> Factory
	{
		get
		{
			return _factory;
		}
		set
		{
			_factory = value as BulletFactorySO;
		}
	}
}

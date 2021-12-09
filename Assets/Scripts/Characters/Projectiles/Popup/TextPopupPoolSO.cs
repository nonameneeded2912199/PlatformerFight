using PlatformerFight.Factory;
using PlatformerFight.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTextPopupPool", menuName = "Pool/TextPopup Pool")]
public class TextPopupPoolSO : ComponentPoolSO<TextPopup>
{
	[SerializeField] private TextPopupFactorySO _factory;

	public override IFactory<TextPopup> Factory
	{
		get
		{
			return _factory;
		}
		set
		{
			_factory = value as TextPopupFactorySO;
		}
	}
}

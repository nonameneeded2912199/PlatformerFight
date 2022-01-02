using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Vector2 Event Channel")]
public class Vector2EventChannelSO : DescriptionBaseSO
{
	public UnityAction<Vector2> OnEventRaised;

	public void RaiseEvent(Vector2 value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}

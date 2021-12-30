using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Long Event Channel")]
public class LongEventChannelSO : DescriptionBaseSO
{
	public UnityAction<long> OnEventRaised;

	public void RaiseEvent(long value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Phase Result Event Channel")]
public class PhaseResultEventChannelSO : DescriptionBaseSO
{
    public UnityAction<bool, long> OnEventRaised;

    public void RaiseEvent(bool success, long value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(success, value);
    }
}

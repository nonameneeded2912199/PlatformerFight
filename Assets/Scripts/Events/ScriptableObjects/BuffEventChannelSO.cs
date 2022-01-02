using PlatformerFight.Buffs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Buff Event Channel")]
public class BuffEventChannelSO : DescriptionBaseSO
{
    public UnityAction<BaseBuff> OnRaiseEvent;

    public void RaiseEvent(BaseBuff value)
    {
        if (OnRaiseEvent != null)
            OnRaiseEvent.Invoke(value);
    }
}

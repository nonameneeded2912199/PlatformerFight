using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Boss Phase Event Channel")]
public class BossPhaseEventChannelSO : DescriptionBaseSO
{
    public UnityAction<BossPhaseDataSO> OnEventRaised;

    public void RaiseEvent(BossPhaseDataSO value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}

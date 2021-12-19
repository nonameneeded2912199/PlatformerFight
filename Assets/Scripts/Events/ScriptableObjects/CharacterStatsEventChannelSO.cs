using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Character Stat Event Channel")]
public class CharacterStatsEventChannelSO : ScriptableObject
{
    public UnityAction<CharacterStats> OnEventRaised;

    public void RaiseEvent(CharacterStats value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}

using PlatformerFight.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Skill Event Channel")]
public class SkillEventChannelSO : ScriptableObject
{
    public UnityAction<Skill> OnEventRaised;

    public void RaiseEvent(Skill value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}

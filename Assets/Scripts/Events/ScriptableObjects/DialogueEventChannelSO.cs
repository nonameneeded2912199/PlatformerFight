using System;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that have one Text Asset argument.
/// Example: Spawn system initializes player and fire event, where the transform is the position of player.
/// </summary>

[CreateAssetMenu(menuName = "Events/Dialogue Event Channel")]
public class DialogueEventChannelSO : DescriptionBaseSO
{
	public UnityAction<TextAsset, Action> OnEventRaised;

	public void RaiseEvent(TextAsset value, Action action = null)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value, action);
	}
}

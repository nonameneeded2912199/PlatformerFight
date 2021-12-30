using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Difficulty Event Channel")]
public class GameDifficultyEventChannelSO : DescriptionBaseSO
{
    public delegate GameDifficulty RequestDifficultyAction();

    public RequestDifficultyAction OnRequestDifficultyAction;

    public GameDifficulty RaiseDifficultyEvent()
    {
        GameDifficulty difficulty = default;

        if (OnRequestDifficultyAction != null)
        {
            difficulty = OnRequestDifficultyAction.Invoke();
        }
        else
        {
            Debug.LogWarning("An Difficulty play event was requested but nobody picked it up. ");
        }

        return difficulty;
    }
}

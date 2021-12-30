using UnityEngine;

[CreateAssetMenu(menuName = "Events/Save Event Channel")]
public class SaveEventChannelSO : ScriptableObject
{
    public delegate void RequestSaveAction(int charID, int difficulty, int lives, string stageID, long score);

    public RequestSaveAction OnSaveRequested;

    public void RaiseSaveAction(int charID, int difficulty, int lives, string stageID, long score)
    {
        if (OnSaveRequested != null)
        {
            OnSaveRequested.Invoke(charID, difficulty, lives, stageID, score);
        }
        else
        {
            Debug.LogWarning("An Save play event was requested but nobody picked it up. ");
        }
    }
}

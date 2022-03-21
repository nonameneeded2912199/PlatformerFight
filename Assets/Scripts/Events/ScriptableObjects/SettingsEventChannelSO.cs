using UnityEngine;

[CreateAssetMenu(menuName = "Events/Settings Event Channel")]
public class SettingsEventChannelSO : DescriptionBaseSO
{
    public delegate void RequestSettingsAction(float bgmVolume, float sfxVolume);

    public RequestSettingsAction OnSettingsRequested;

    public void RaiseSaveAction(float bgmVolume, float sfxVolume)
    {
        if (OnSettingsRequested != null)
        {
            OnSettingsRequested.Invoke(bgmVolume, sfxVolume);
        }
        else
        {
            Debug.LogWarning("An Settings play event was requested but nobody picked it up. ");
        }
    }
}


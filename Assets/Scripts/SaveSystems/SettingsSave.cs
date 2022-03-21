using UnityEngine;

[System.Serializable]
public class SettingsSave
{
    public float _bgmVolume = default;
    public float _sfxVolume = default;

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}

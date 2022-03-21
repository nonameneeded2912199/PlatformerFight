using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "NewSaveSystemSO", menuName = "Save System/Save System SO")]
public class SaveSystem : ScriptableObject
{
    public string saveFilename = "save.file";
    public string backupSaveFilename = "save.file.bak";

    public string settingsFilename = "settings.file";
    public string backupSettingsFilename = "settings.file.bak";

    [SerializeField]
    private SaveEventChannelSO _onSaveRequested = default;

    [SerializeField]
    private SettingsEventChannelSO _onSettingsRequested = default;

    public Save saveData = new Save();
    public SettingsSave settingsData = new SettingsSave();

    private void OnEnable()
    {
        _onSaveRequested.OnSaveRequested += SaveDataToDisk;
        _onSettingsRequested.OnSettingsRequested += SaveSettingsToDisk;
    }

    private void OnDisable()
    {
        _onSaveRequested.OnSaveRequested -= SaveDataToDisk;
        _onSettingsRequested.OnSettingsRequested -= SaveSettingsToDisk;
    }

    private void CacheLoadStage(GameSceneSO stage, bool showLoadingScreen, bool fadeScreen)
    {
        StageSO stageSO = stage as StageSO;
        if (stageSO)
        {
            saveData._stageID = stageSO.Guid;
        }
    }

    public bool LoadSaveDataFromDisk()
    {
        if (FileManager.LoadFromFile(saveFilename, out var json))
        {
            saveData.LoadFromJson(json);
            return true;
        }

        return false;
    }

    public bool LoadSettingsDataFromDisk()
    {
        if (FileManager.LoadFromFile(settingsFilename, out var json))
        {
            settingsData.LoadFromJson(json);
            return true;
        }

        return false;
    }    

    private void SaveDataToDisk(int charID, int difficulty, int lives, string stageID, long score)
    {
        saveData._charID = charID;
        saveData._score = score;
        saveData._stageID = stageID;
        saveData._difficulty = difficulty;
        saveData._lives = lives;

        if (FileManager.MoveFile(saveFilename, backupSaveFilename))
        {
            if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
            {
                Debug.Log(saveFilename + " saved successfully.");
            }
        }
    }

    private void SaveSettingsToDisk(float bgmVolume, float sfxVolume)
    {
        settingsData._bgmVolume = bgmVolume;
        settingsData._sfxVolume = sfxVolume;

        if (FileManager.MoveFile(settingsFilename, backupSettingsFilename))
        {
            if (FileManager.WriteToFile(settingsFilename, settingsData.ToJson()))
            {
                Debug.Log("Settings " + saveFilename + " saved successfully.");
            }
        }    
    }    

    public void WriteEmptySaveFile()
    {
        FileManager.WriteToFile(saveFilename, "");
    }

    public void SetNewGameData(int charID, int difficulty, string stageID)
    {
        FileManager.WriteToFile(saveFilename, "");

        SaveDataToDisk(charID, difficulty, 2, stageID, 0);
    }

    public void SetNewSettingsData()
    {
        FileManager.WriteToFile(settingsFilename, "");

        SaveSettingsToDisk(1.0f, 1.0f);
    }
}

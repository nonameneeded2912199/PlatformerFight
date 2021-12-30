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

    [SerializeField]
    private SaveEventChannelSO _onSaveRequested = default;

    public Save saveData = new Save();

    private void OnEnable()
    {
        _onSaveRequested.OnSaveRequested += SaveDataToDisk;
    }

    private void OnDisable()
    {
        _onSaveRequested.OnSaveRequested -= SaveDataToDisk;
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

    public void WriteEmptySaveFile()
    {
        FileManager.WriteToFile(saveFilename, "");
    }

    public void SetNewGameData(int charID, int difficulty, string stageID)
    {
        FileManager.WriteToFile(saveFilename, "");

        SaveDataToDisk(charID, difficulty, 2, stageID, 0);
    }
}

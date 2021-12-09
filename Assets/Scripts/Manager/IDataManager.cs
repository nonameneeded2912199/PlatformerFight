using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataManager
{
    void SetDataToSave();
}

[Serializable]
public class PlayerData : IDataManager
{
    public static readonly string CHECKPOINT_NAME = "CHECKPOINT_NAME";

    public static readonly string SAVED_DIFFICULTY = "SAVED_DIFFICULTY";
    public static readonly string SCENE_SAVED = "SCENE_SAVED";

    public string checkPointName;

    public int difficulty;

    public string sceneName;

    public void SetDataToSave()
    {
        checkPointName = DataOnCheckPoint.CheckPointName;

        //difficulty = (int)GameManager.Instance.currentGameDifficulty;

        sceneName = DataOnCheckPoint.sceneName;
    }
}

public static class DataOnCheckPoint
{
    public static string CheckPointName { get; set; }

    public static string sceneName { get; set; }

    public static string LastVCamName { get; set; }
}

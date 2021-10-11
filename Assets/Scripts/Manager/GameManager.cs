using Cinemachine;
using Core.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private CanvasGroup imageBlackScreen;

    public GameDifficulty currentGameDifficulty = GameDifficulty.NORMAL;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);  
    }

    void Update()
    {
        GameEventManager.Update();
    }

    public void RestartLevel()
    {
        ScenesManager.Instance.RestartScene();
    }

    public void RestartLevelFinished()
    {
        StageManager thisStageManager = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>();

        ICinemachineCamera startCamera = thisStageManager.stageCamera;
        startCamera.VirtualCameraGameObject.SetActive(true);

        Vector3 spawnPoint;

        Checkpoint cpoint = thisStageManager.checkPoint;

        if (cpoint != null)
        {
            if (cpoint.CheckPointName == SaveManager.CheckPointName)
            {
                spawnPoint = cpoint.transform.position;
                //CharacterController.Instance.Respawn(spawnPoint);
            }
        }

        PoolManager.Instance.ResetPool();
    }
}

public enum GameDifficulty
{
    EASY,
    NORMAL,
    HARD,
    LUNATIC
}

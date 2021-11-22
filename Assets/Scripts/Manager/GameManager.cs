using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private CanvasGroup imageBlackScreen;

    public GameDifficulty currentGameDifficulty = GameDifficulty.NORMAL;

    private GameObject bullet;

    private GameObject damagePopup;
    public GameObject CommonBullet
    {
        get => bullet;
    }

    public GameObject DamagePopup
    {
        get => damagePopup;
    }    

    /*protected override*/
    void Awake()
    {
        //base.Awake();
        //DontDestroyOnLoad(gameObject);  
        if (bullet == null)
        {
            Addressables.LoadAssetAsync<GameObject>("CommonBullet").Completed += AddressableAsyncLoadBullet;
        }

        if (damagePopup == null)
        {
            Addressables.LoadAssetAsync<GameObject>("CommonPopup").Completed += AddressableAsyncLoadPopup;
        }
    }

    private void AddressableAsyncLoadBullet(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> asyncOperation)
    {
        // Set things
        bullet = asyncOperation.Result;


        // Unregister event & Release asset
        asyncOperation.Completed -= AddressableAsyncLoadBullet;
        Addressables.Release(asyncOperation);
    }

    private void AddressableAsyncLoadPopup(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> asyncOperation)
    {
        // Set things
        damagePopup = asyncOperation.Result;


        // Unregister event & Release asset
        asyncOperation.Completed -= AddressableAsyncLoadPopup;
        Addressables.Release(asyncOperation);
    }

    void Update()
    {
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

        //PoolManager.Instance.ResetPool();
    }
}

public enum GameDifficulty
{
    EASY,
    NORMAL,
    HARD,
    LUNATIC
}

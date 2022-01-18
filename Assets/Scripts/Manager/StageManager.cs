using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private MenuSO _preScene = default;

    [SerializeField]
    private GameSceneSO _thisScene = default;

    [SerializeField]
    private GameSceneSO _nextScene = default;

    [SerializeField]
    private GameObject[] passages = default;

    [Header("Event Channels")]
    [SerializeField]
    private VoidEventChannelSO _onEnemyDefeated = default;

    [SerializeField]
    private VoidEventChannelSO _onRestartStage = default;

    [SerializeField]
    private VoidEventChannelSO _onStageCompleted = default;

    [SerializeField]
    private IntEventChannelSO _onOpenDoor = default;

    [SerializeField]
    private LoadEventChannelSO _loadScene = default;

    [SerializeField]
    private GameStateSO _gameStateSO = default;

    [SerializeField]
    private SaveEventChannelSO _onRequestSave = default;

    /// <summary>
    /// If numEnemies is zero, small enemies in this stage will no longer yield scores until you restart the stage;
    /// </summary>
    [Space]
    [Header("Number of enemies that yield points")]
    [SerializeField]
    private int numEnemies = 0;

    private void Awake()
    {
        _gameStateSO.SelectStage(_thisScene as StageSO);
    }

    private void OnEnable()
    {
        _onRestartStage.OnEventRaised += RestartStage;
        _onStageCompleted.OnEventRaised += MoveToNextScene;
        _onEnemyDefeated.OnEventRaised += SmallEnemiesDefeated;
        _onOpenDoor.OnEventRaised += OpenDoor;
    }

    private void OnDisable()
    {
        _onRestartStage.OnEventRaised -= RestartStage;
        _onStageCompleted.OnEventRaised -= MoveToNextScene;
        _onEnemyDefeated.OnEventRaised -= SmallEnemiesDefeated;
        _onOpenDoor.OnEventRaised -= OpenDoor;
    }

    private void RestartStage()
    {
        _loadScene.RaiseEvent(_preScene, true, true);
    }

    private void MoveToNextScene()
    {
        PlayerPrefs.DeleteAll();

        StartCoroutine(MoveSceneCoroutine());
    }

    private IEnumerator MoveSceneCoroutine()
    {
        yield return new WaitForSeconds(10f);

        if (_nextScene is StageSO)
        {
            StageSO nextStage = _nextScene as StageSO;
            _onRequestSave.RaiseSaveAction(_gameStateSO.ChosenPlayerID, (int)_gameStateSO.CurrentDifficulty,
                _gameStateSO.LifeCount, nextStage.Guid, _gameStateSO.Score);
            _gameStateSO.SelectStage(nextStage);
            _loadScene.RaiseEvent(_preScene, true, true);
        }
        else
        {
            _loadScene.RaiseEvent(_nextScene, true, true);
        }
    }

    public void OpenDoor(int doorNumber)
    {
        if (doorNumber < 0 || doorNumber > passages.Length - 1)
        {
            Debug.LogError("Passage way doesn't exist");
            return;
        }

        passages[doorNumber].SetActive(false);
    }    

    private void SmallEnemiesDefeated()
    {
        if (numEnemies <= 0)
            return;

        numEnemies--;
        if (numEnemies <= 0)
        {
            EnemySpawner.enemyHoldScore = false;
        }
    }
        
}

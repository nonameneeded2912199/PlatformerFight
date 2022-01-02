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

    [Header("Event Channels")]
    [SerializeField]
    private VoidEventChannelSO _onRestartStage = default;

    [SerializeField]
    private VoidEventChannelSO _onStageCompleted = default;

    [SerializeField]
    private LoadEventChannelSO _loadScene = default;

    [SerializeField]
    private GameStateSO _gameStateSO = default;

    [SerializeField]
    private SaveEventChannelSO _onRequestSave = default;

    private void Awake()
    {
        _gameStateSO.SelectStage(_thisScene as StageSO);
    }

    private void OnEnable()
    {
        _onRestartStage.OnEventRaised += RestartStage;
        _onStageCompleted.OnEventRaised += MoveToNextScene;
    }

    private void OnDisable()
    {
        _onRestartStage.OnEventRaised -= RestartStage;
        _onStageCompleted.OnEventRaised -= MoveToNextScene;
    }

    private void RestartStage()
    {
        _loadScene.RaiseEvent(_preScene, true, true);
    }

    private void MoveToNextScene()
    {
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
        
}

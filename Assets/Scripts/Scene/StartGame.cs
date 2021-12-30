using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private StageSO stageToLoad = default;

    [SerializeField]
    private MenuSO preScene = default;

    [SerializeField]
    private SaveSystem _saveSystem = default;

    [SerializeField]
    private bool showLoadScreen = default;

    [Header("Broadcasting on")]
    [SerializeField] 
    private LoadEventChannelSO loadScene = default;

    [Header("Listening to")]
    [SerializeField] 
    private VoidEventChannelSO onNewGameButton = default;
    [SerializeField] 
    private VoidEventChannelSO onContinueButton = default;
    [SerializeField] 
    private GameStateSO gameStateSO = default;

    private bool _hasSaveData;

    // Start is called before the first frame update
    void Start()
    {
        _hasSaveData = _saveSystem.LoadSaveDataFromDisk();
        onNewGameButton.OnEventRaised += StartNewGame;
        onContinueButton.OnEventRaised += ContinueGame;
    }

    // Update is called once per frame
    void OnDestroy()
    {
        onNewGameButton.OnEventRaised -= StartNewGame;
        onContinueButton.OnEventRaised -= ContinueGame;
    }

    private void StartNewGame()
    {
        _hasSaveData = false;

        gameStateSO.SelectStage(stageToLoad);
        gameStateSO.LifeCount = 2;
        gameStateSO.Score = 0;
        _saveSystem.SetNewGameData(gameStateSO.ChosenPlayerID, (int)gameStateSO.CurrentDifficulty, gameStateSO.CurrentStage.Guid);
        loadScene.RaiseEvent(preScene, showLoadScreen);
    }

    private void ContinueGame()
    {
        StartCoroutine(LoadSaveGame());
    }

    private IEnumerator LoadSaveGame()
    {
        var asyncOperationHandle = Addressables.LoadAssetAsync<StageSO>(_saveSystem.saveData._stageID);

        yield return asyncOperationHandle;

        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            gameStateSO.SelectCharacter(_saveSystem.saveData._charID);
            gameStateSO.Score = _saveSystem.saveData._score;
            gameStateSO.LifeCount = _saveSystem.saveData._lives;
            gameStateSO.SetDifficulty((GameDifficulty)_saveSystem.saveData._difficulty);
            gameStateSO.SelectStage(asyncOperationHandle.Result);
            loadScene.RaiseEvent(preScene, showLoadScreen);
        }
    }
}

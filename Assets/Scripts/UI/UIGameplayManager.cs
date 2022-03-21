using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class UIGameplayManager : MonoBehaviour
{

	[Header("Scene UI")]
	[SerializeField]
	private UI_Pause _pauseScreen = default;

	[SerializeField]
	private UI_GameOver _gameOverScreen = default;

	[SerializeField]
	private UI_DialogueManager _dialogueScreen = default;

	[SerializeField]
	private SaveSystem _saveSystem;

	[Header("Gameplay")]
	[SerializeField] 
	private GameStateSO _gameState = default;
	[SerializeField] 
	private MenuSO _mainMenu = default;
	[SerializeField]
	private MenuSO preScene = default;
	[SerializeField]
	private InputReader _inputReader = default;
	[SerializeField]
	private Button pauseButton = default;
	

	[Header("Event channels")]
	[SerializeField]
	private VoidEventChannelSO _onSceneReady = default;

	[SerializeField]
	private LoadEventChannelSO _loadMenuEvent = default;

	[SerializeField]
	private DialogEventChannelSO _onDialogRequested = default;

	[SerializeField]
	private VoidEventChannelSO _loadGameOverScreen = default;

	[SerializeField]
	private DialogueEventChannelSO _onShowDialogue = default;

	[SerializeField]
	private UI_SettingsPanel settingsPanel = default;

	private void OnEnable()
    {
		_onSceneReady.OnEventRaised += ResetUI;
		_inputReader.UIPauseEvent += OpenUIPause;
		pauseButton.onClick.AddListener(OpenUIPause);

		_loadGameOverScreen.OnEventRaised += ShowGameOverScreen;
		_onShowDialogue.OnEventRaised += OpenUIDialogue;
    }

    private void OnDisable()
    {
		_onSceneReady.OnEventRaised -= ResetUI;
		_inputReader.UIPauseEvent -= OpenUIPause;
		pauseButton.onClick.RemoveAllListeners();
		_loadGameOverScreen.OnEventRaised -= ShowGameOverScreen;
		_onShowDialogue.OnEventRaised -= OpenUIDialogue;
	}

    private void ResetUI()
    {
		_pauseScreen.gameObject.SetActive(false);

		Time.timeScale = 1;
    }

	public void OpenSettingsScreen()
	{
		settingsPanel.gameObject.SetActive(true);
		settingsPanel.OnCloseSettingsPanel += CloseSettingsScreen;

	}
	public void CloseSettingsScreen()
	{
		settingsPanel.OnCloseSettingsPanel -= CloseSettingsScreen;
		settingsPanel.gameObject.SetActive(false);
	}

	private void OpenUIPause()
    {
		_inputReader.UIPauseEvent -= OpenUIPause;

		Time.timeScale = 0;

		_pauseScreen.Resumed += CloseUIPause;

		_pauseScreen.SettingsOpened += OpenSettingsScreen;

		_pauseScreen.BackToMainRequested += BackToMainMenu;

		_pauseScreen.gameObject.SetActive(true);
		_inputReader.EnableUIInput();
    }

	private void OpenUIDialogue(TextAsset dialogueData, Action afterDialogue = null)
    {
		_dialogueScreen.gameObject.SetActive(true);

		_dialogueScreen.EnterDialogueMode(dialogueData, afterDialogue);

		_inputReader.EnableDialogueInput();
    }		

	private void CloseUIPause()
    {
		Time.timeScale = 1;

		_inputReader.UIPauseEvent += OpenUIPause;

		_pauseScreen.Resumed -= CloseUIPause;

		_pauseScreen.SettingsOpened -= OpenUIPause;

		_pauseScreen.BackToMainRequested -= BackToMainMenu;

		_pauseScreen.gameObject.SetActive(false);

		_inputReader.EnableGameplayInput();
	}

	private void BackToMainMenu()
    {
		_onDialogRequested.RaiseSaveAction().SetTitle("Back to menu?")
			.SetText("Go back to the menu? Unsaved progress will be lost.")
			.AddButton("Yes", () => 
			{
				CloseUIPause();
				PlayerPrefs.DeleteAll();
				_loadMenuEvent.RaiseEvent(_mainMenu, true, true);
			})
			.AddCancelButton("No")
			.Show();	
    }

	private void ShowGameOverScreen()
    {
		Time.timeScale = 0;
		_gameOverScreen.ResumedFromSave += ResumeFromSavePoint;
		_gameOverScreen.BackToMenu += BackToMainMenuGG;
		_gameOverScreen.gameObject.SetActive(true);
		_inputReader.EnableUIInput();
	}

	public void ResumeFromSavePoint()
	{
		Time.timeScale = 1;
		_gameOverScreen.ResumedFromSave -= ResumeFromSavePoint;
		_gameOverScreen.BackToMenu -= BackToMainMenuGG;
		StartCoroutine(LoadSaveGame());
	}

	public void BackToMainMenuGG()
	{
		_onDialogRequested.RaiseSaveAction().SetTitle("Back to menu?")
			.SetText("Go back to the menu? Unsaved progress will be lost.")
			.AddButton("Yes", () =>
			{
				Time.timeScale = 1;
				PlayerPrefs.DeleteAll();
				_gameOverScreen.ResumedFromSave -= ResumeFromSavePoint;
				_gameOverScreen.BackToMenu -= BackToMainMenuGG;
				_loadMenuEvent.RaiseEvent(_mainMenu, true, true);
			})
			.AddCancelButton("No")
			.Show();
	}

	private IEnumerator LoadSaveGame()
	{
		var asyncOperationHandle = Addressables.LoadAssetAsync<StageSO>(_saveSystem.saveData._stageID);

		yield return asyncOperationHandle;

		if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
		{
			PlayerPrefs.DeleteAll();

			_gameState.SelectCharacter(_saveSystem.saveData._charID);
			_gameState.Score = _saveSystem.saveData._score;
			_gameState.LifeCount = _saveSystem.saveData._lives;
			_gameState.SetDifficulty((GameDifficulty)_saveSystem.saveData._difficulty);
			_gameState.SelectStage(asyncOperationHandle.Result);
			_loadMenuEvent.RaiseEvent(preScene, true, true);
		}
	}
}

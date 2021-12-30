using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplayManager : MonoBehaviour
{

	[Header("Scene UI")]
	[SerializeField]
	private UI_Pause _pauseScreen = default;

	[Header("Gameplay")]
	[SerializeField] private GameStateSO _gameStateManager = default;
	[SerializeField] private MenuSO _mainMenu = default;
	[SerializeField] private InputReader _inputReader = default;
	[SerializeField]
	private Button pauseButton = default;
	//[SerializeField] private ActorSO _mainProtagonist = default;

	[Header("Event channels")]
	[SerializeField]
	private VoidEventChannelSO _onSceneReady = default;

	[SerializeField]
	private LoadEventChannelSO _loadMenuEvent = default;

	[SerializeField]
	private DialogEventChannelSO _onDialogRequested = default;

    private void OnEnable()
    {
		_onSceneReady.OnEventRaised += ResetUI;
		_inputReader.UIPauseEvent += OpenUIPause;
		pauseButton.onClick.AddListener(OpenUIPause);
    }

    private void OnDisable()
    {
		_onSceneReady.OnEventRaised -= ResetUI;
		_inputReader.UIPauseEvent -= OpenUIPause;
		pauseButton.onClick.RemoveAllListeners();
    }

    private void ResetUI()
    {
		_pauseScreen.gameObject.SetActive(false);

		Time.timeScale = 1;
    }

	private void OpenUIPause()
    {
		_inputReader.UIPauseEvent -= OpenUIPause;

		Time.timeScale = 0;

		_pauseScreen.Resumed += CloseUIPause;

		_pauseScreen.SettingsOpened += CloseUIPause;

		_pauseScreen.BackToMainRequested += BackToMainMenu;

		_pauseScreen.gameObject.SetActive(true);
		_inputReader.EnableUIInput();
    }

	private void CloseUIPause()
    {
		Time.timeScale = 1;

		_inputReader.UIPauseEvent += OpenUIPause;

		_pauseScreen.Resumed -= CloseUIPause;

		_pauseScreen.SettingsOpened -= CloseUIPause;

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
				_loadMenuEvent.RaiseEvent(_mainMenu, true, true);
			})
			.AddCancelButton("No")
			.Show();	
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplayManager : MonoBehaviour
{
	[Header("Scene UI")]
	[SerializeField]
	private UI_Pause _pauseScreen = default;

	[Header("Gameplay")]
	[SerializeField] private GameStateSO _gameStateManager = default;
	[SerializeField] private MenuSO _mainMenu = default;
	[SerializeField] private InputReader _inputReader = default;
	//[SerializeField] private ActorSO _mainProtagonist = default;

	[Header("Event channels")]
	[SerializeField]
	private VoidEventChannelSO _onSceneReady = default;

	[SerializeField]
	private LoadEventChannelSO _loadMenuEvent = default;

    private void OnEnable()
    {
		_onSceneReady.OnEventRaised += ResetUI;
		_inputReader.UIPauseEvent += OpenUIPause;
    }

    private void OnDisable()
    {
		_onSceneReady.OnEventRaised -= ResetUI;
		_inputReader.UIPauseEvent -= OpenUIPause;
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
		_gameStateManager.UpdateGameState(GameState.Pause);
    }

	private void CloseUIPause()
    {
		Time.timeScale = 1;

		_inputReader.UIPauseEvent += OpenUIPause;

		_pauseScreen.Resumed -= CloseUIPause;

		_pauseScreen.SettingsOpened -= CloseUIPause;

		_pauseScreen.BackToMainRequested -= BackToMainMenu;

		_pauseScreen.gameObject.SetActive(false);

		_gameStateManager.ResetToPreviousGameState();

		if (_gameStateManager.CurrentGameState == GameState.Gameplay)
        {
			_inputReader.EnableGameplayInput();
        }			
    }

	private void BackToMainMenu()
    {
		CloseUIPause();
		_loadMenuEvent.RaiseEvent(_mainMenu, false);
    }
}

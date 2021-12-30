using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MenuManager : MonoBehaviour
{
	//[SerializeField] private UISettingsController _settingsPanel = default;
	//[SerializeField] private UICredits _creditsPanel = default;
	[SerializeField]
	private UI_DifficultSelectorPanel difficultyPanel = default;

	[SerializeField]
	private UI_CharacterSelector characterSelect = default;

	[SerializeField] 
	private UI_MainMenu _mainMenuPanel = default;

	[SerializeField] private SaveSystem _saveSystem = default;

	[SerializeField] 
	private InputReader _inputReader = default;


	[Header("Broadcasting on")]
	[SerializeField]
	private VoidEventChannelSO _continueGameEvent = default;
	[SerializeField]
	private DialogEventChannelSO _onRequestDialog = default;



	private bool _hasSaveData;

	private IEnumerator Start()
	{
		_inputReader.EnableUIInput();
		yield return new WaitForSeconds(0.4f); //waiting time for all scenes to be loaded 
		SetMenuScreen();
	}
	void SetMenuScreen()
	{
		_hasSaveData = _saveSystem.LoadSaveDataFromDisk();
		_mainMenuPanel.SetMenuScreen(_hasSaveData);
		_mainMenuPanel.ContinueButtonAction += _continueGameEvent.RaiseEvent;
		_mainMenuPanel.NewGameButtonAction += ButtonStartNewGameClicked;
		_mainMenuPanel.SettingsButtonAction += OpenSettingsScreen;
		_mainMenuPanel.CreditsButtonAction += OpenCreditsScreen;
		_mainMenuPanel.ExitButtonAction += ShowExitConfirmationPopup;
	}

	void ButtonStartNewGameClicked()
	{
		if (!_hasSaveData)
		{
			ConfirmStartNewGame();

		}
		else
		{
			ShowStartNewGameConfirmationPopup();
		}
	}

	void ConfirmStartNewGame()
	{
		OpenDifficultySelect();
	}

	void ShowStartNewGameConfirmationPopup()
	{
		_onRequestDialog.RaiseSaveAction().SetTitle("Save data exists")
			.SetText("Saved data exists. Are you sure you want to start a new game?" + " \n " + "This will delete your previously saved data.")
			.AddButton("Yes", () => { OpenDifficultySelect(); })
			.AddCancelButton("No")
			.Show();
	}

	public void OpenDifficultySelect()
    {
		difficultyPanel.gameObject.SetActive(true);
		difficultyPanel.OnChooseDifficulty += OpenCharacterSelect;
		difficultyPanel.OnCloseDifficultyPanel += CloseDifficultySelect;
    }

	public void OpenCharacterSelect()
    {
		characterSelect.gameObject.SetActive(true);
		characterSelect.OnCloseCharacterSelectPanel += OpenDifficultySelect;
    }

	public void CloseDifficultySelect()
    {
		difficultyPanel.OnCloseDifficultyPanel -= CloseDifficultySelect;
		difficultyPanel.OnChooseDifficulty -= OpenCharacterSelect;
		difficultyPanel.gameObject.SetActive(false);
    }

	public void CloseCharacterSelect()
	{		
		characterSelect.OnCloseCharacterSelectPanel -= OpenDifficultySelect;
		characterSelect.gameObject.SetActive(false);
	}

	public void OpenSettingsScreen()
	{
		//_settingsPanel.gameObject.SetActive(true);
		//_settingsPanel.Closed += CloseSettingsScreen;

	}
	public void CloseSettingsScreen()
	{
		//_settingsPanel.Closed -= CloseSettingsScreen;
		//_settingsPanel.gameObject.SetActive(false);
		_mainMenuPanel.SetMenuScreen(_hasSaveData);

	}
	public void OpenCreditsScreen()
	{
		//_creditsPanel.gameObject.SetActive(true);

		//_creditsPanel.OnCloseCredits += CloseCreditsScreen;




	}
	public void CloseCreditsScreen()
	{
		//_creditsPanel.OnCloseCredits -= CloseCreditsScreen;
		//_creditsPanel.gameObject.SetActive(false);
		_mainMenuPanel.SetMenuScreen(_hasSaveData);

	}


	public void ShowExitConfirmationPopup()
	{
		_onRequestDialog.RaiseSaveAction().SetTitle("Confirm quit game")
			.SetText("Quit game?")
			.AddButton("Yes", () => { Application.Quit(); })
			.AddCancelButton("No")
			.Show();
	}

	private void OnDestroy()
	{
		//_popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
		//_popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;

	}
}

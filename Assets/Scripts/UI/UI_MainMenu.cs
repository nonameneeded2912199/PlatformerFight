using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
	public UnityAction NewGameButtonAction;
	public UnityAction ContinueButtonAction;
	public UnityAction SettingsButtonAction;
	public UnityAction CreditsButtonAction;
	public UnityAction ExitButtonAction;

	[SerializeField] 
	private Button _continueButton = default;
	[SerializeField] 
	private Button _NewGameButton = default;

	public void SetMenuScreen(bool hasSaveData)
	{
		_continueButton.interactable = hasSaveData;
		_NewGameButton.Select();
	}

	public void NewGameButton()
	{
		NewGameButtonAction.Invoke();
	}

	public void ContinueButton()
	{
		ContinueButtonAction.Invoke();
	}

	public void SettingsButton()
	{
		SettingsButtonAction.Invoke();
	}

	public void CreditsButton()
	{
		CreditsButtonAction.Invoke();
	}

	public void ExitButton()
	{
		ExitButtonAction.Invoke();
	}
}

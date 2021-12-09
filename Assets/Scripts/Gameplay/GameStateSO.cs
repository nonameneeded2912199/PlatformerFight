using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
	Gameplay, //regular state: player moves, attacks, can perform actions
	Pause, //pause menu is opened, the whole game world is frozen
	Inventory, //when inventory UI or cooking UI are open
	Dialogue,
	Cutscene,
	LocationTransition, //when the character steps into LocationExit trigger, fade to black begins and control is removed from the player
	//Combat, //enemy is nearby and alert, player can't open Inventory or initiate dialogues, but can pause the game
}

public enum GameDifficulty
{
	EASY,
	NORMAL,
	HARD,
	LUNATIC
}

[CreateAssetMenu(fileName = "GameState", menuName = "Gameplay/GameState", order = 51)]
public class GameStateSO : DescriptionBaseSO
{
	public GameState CurrentGameState => _currentGameState;
	public GameDifficulty CurrentDifficulty => currentDifficulty;

	[Header("Game states")]
	[SerializeField] /*[ReadOnly]*/ private GameState _currentGameState = default;
	[SerializeField] /*[ReadOnly]*/ private GameState _previousGameState = default;

	[Header("Game difficulty")]
	[SerializeField] private GameDifficulty currentDifficulty = default;

	/*[Header("Broadcasting on")]
	[SerializeField] private BoolEventChannelSO _onCombatStateEvent = default;*/

	private void Start()
	{
	}

	public void SetDifficulty(GameDifficulty difficulty)
    {
		if (currentDifficulty == difficulty)
			return;

		currentDifficulty = difficulty;
    }		

	public void UpdateGameState(GameState newGameState)
	{
		if (newGameState == CurrentGameState)
			return;

		_previousGameState = _currentGameState;
		_currentGameState = newGameState;
	}

	public void ResetToPreviousGameState()
	{
		if (_previousGameState == _currentGameState)
			return;

		GameState stateToReturnTo = _previousGameState;
		_previousGameState = _currentGameState;
		_currentGameState = stateToReturnTo;
	}
}

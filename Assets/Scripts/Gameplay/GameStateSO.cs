using System.Collections.Generic;
using UnityEngine;

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
    #region Properties
	public GameDifficulty CurrentDifficulty => currentDifficulty;

	public int ChosenPlayerID => chosenPlayerID;

	public StageSO CurrentStage => currentStage;

	public int LifeCount { get; set; } = default;

	public long Score { get; set; } = default;

    #endregion

	[Header("Game difficulty")]
	[SerializeField] private GameDifficulty currentDifficulty = default;

	[Header("Selected protagonist")]
	[SerializeField]
	private int chosenPlayerID;

	[Header("Current Stage")]
	[SerializeField]
	private StageSO currentStage = default;

	public void SetDifficulty(GameDifficulty difficulty)
    {
		if (currentDifficulty == difficulty)
			return;

		currentDifficulty = difficulty;
    }		

	public void SelectCharacter(int chosenPlayerID)
    {
		this.chosenPlayerID = chosenPlayerID;
    }

	public void SelectStage(StageSO stage)
    {
		this.currentStage = stage;
    }	
}

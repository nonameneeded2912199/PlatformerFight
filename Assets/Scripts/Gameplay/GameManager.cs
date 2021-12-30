using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameStateSO _gameState = default;

    private GameDifficulty currentDifficulty = default;

    [SerializeField]
    private long[] extraLifeThreshold;

    [SerializeField]
    private int currentThreshold = 0;

    [SerializeField]
    private bool maxThresholdReached = false;

    [Header("Event channels")]
    [SerializeField]
    private GameDifficultyEventChannelSO OnRequestDifficulty = default;

    [SerializeField]
    private VoidEventChannelSO checkScoreThreshold = default;

    [SerializeField]
    private IntEventChannelSO onAddLives = default;

    void Awake()
    {
        currentDifficulty = _gameState.CurrentDifficulty;

        if (extraLifeThreshold.Length <= 0)
            return;

        Array.Sort(extraLifeThreshold);

        currentThreshold = 0;

        for (int i = extraLifeThreshold.Length - 1; i >= 0; i--)
        {
            Debug.Log(i);
            if (_gameState.Score > extraLifeThreshold[i])
            {
                currentThreshold = i + 1;
                break;
            }
        }

        if (currentThreshold >= extraLifeThreshold.Length)
            maxThresholdReached = true;
    }

    private void OnEnable()
    {
        OnRequestDifficulty.OnRequestDifficultyAction += ReturnDifficulty;
        checkScoreThreshold.OnEventRaised += CheckScoreLives;
    }

    private void OnDisable()
    {
        OnRequestDifficulty.OnRequestDifficultyAction -= ReturnDifficulty;
        checkScoreThreshold.OnEventRaised -= CheckScoreLives;
    }

    public void CheckScoreLives()
    {
        Debug.Log("Here");
        
        if (extraLifeThreshold.Length <= 0)
            return;

        if (maxThresholdReached)
            return;

        for (int i = currentThreshold; i < extraLifeThreshold.Length; i++)
        {
            if (_gameState.Score >= extraLifeThreshold[i])
            {
                currentThreshold++;
                onAddLives.RaiseEvent(1);
            }
        }

        if (currentThreshold >= extraLifeThreshold.Length)
            maxThresholdReached = true;
    }

    public GameDifficulty ReturnDifficulty()
    {
        return currentDifficulty;
    }
}

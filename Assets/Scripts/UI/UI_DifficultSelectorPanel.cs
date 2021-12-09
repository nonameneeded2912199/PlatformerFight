using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_DifficultSelectorPanel : MonoBehaviour
{
    public UnityAction OnCloseDifficultyPanel;

    [SerializeField]
    private VoidEventChannelSO _startNewGameEvent = default;

    [SerializeField]
    private InputReader _inputReader = default;

    [SerializeField]
    private GameStateSO gameStateSO = default;

    private void OnEnable()
    {
        _inputReader.UICancelEvent += CancelDifficultyPanel;
    }

    private void OnDisable()
    {
        _inputReader.UICancelEvent -= CancelDifficultyPanel;
    }

    public void EasyBtn()
    {
        gameStateSO.SetDifficulty(GameDifficulty.EASY);
        _startNewGameEvent.RaiseEvent();
    }

    public void NormalBtn()
    {
        gameStateSO.SetDifficulty(GameDifficulty.NORMAL);
        _startNewGameEvent.RaiseEvent();
    }

    public void HardBtn()
    {
        gameStateSO.SetDifficulty(GameDifficulty.HARD);
        _startNewGameEvent.RaiseEvent();
    }

    public void LunaticBtn()
    {
        gameStateSO.SetDifficulty(GameDifficulty.LUNATIC);
        _startNewGameEvent.RaiseEvent();
    }

    private void CancelDifficultyPanel()
    {
        OnCloseDifficultyPanel.Invoke();
    }
}

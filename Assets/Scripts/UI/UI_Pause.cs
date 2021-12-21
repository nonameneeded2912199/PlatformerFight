using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Pause : MonoBehaviour
{
    [SerializeField]
    private InputReader _inputReader = default;

    [SerializeField]
    private Button _resumeButton = default;

    [SerializeField]
    private Button _settingsButton = default;

    [SerializeField]
    private Button _backToMenuButton = default;

    [Header("Event Channels")]
    [SerializeField]
    private BoolEventChannelSO _onPauseOpened = default;

    public event UnityAction Resumed = default;
    public event UnityAction SettingsOpened = default;
    public event UnityAction BackToMainRequested = default;

    private void OnEnable()
    {
        _onPauseOpened.RaiseEvent(true);

        //_resumeButton.Select();

        _inputReader.UICancelEvent += Resume;
        _resumeButton.onClick.AddListener(Resumed);
        _settingsButton.onClick.AddListener(SettingsOpened);
        _backToMenuButton.onClick.AddListener(BackToMainRequested);
    }

    private void OnDisable()
    {
        _onPauseOpened.RaiseEvent(false);


        _inputReader.UICancelEvent -= Resume;
        _resumeButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.RemoveAllListeners();
        _backToMenuButton.onClick.RemoveAllListeners();
    }

    public void Resume()
    {
        Resumed.Invoke();
    }
}

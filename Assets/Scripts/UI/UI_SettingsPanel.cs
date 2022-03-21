using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_SettingsPanel : MonoBehaviour
{
    [SerializeField]
    Slider bgmSlider, sfxSlider;

    [SerializeField]
    private InputReader _inputReader = default;

    [SerializeField]
    private SettingsEventChannelSO _onSettingsRequested = default;

    [SerializeField]
    private SaveSystem _saveSystem = default;

    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change SFXs volume")]
    [SerializeField] private FloatEventChannelSO _SFXVolumeEventChannel = default;
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Music volume")]
    [SerializeField] private FloatEventChannelSO _musicVolumeEventChannel = default;

    public UnityAction OnCloseSettingsPanel;

    private void Awake()
    {
        bgmSlider.maxValue = 1;
        bgmSlider.minValue = 0;
        sfxSlider.maxValue = 1;
        sfxSlider.minValue = 0;
    }

    private void OnEnable()
    {
        _inputReader.UICancelEvent += CancelBtn;
        bgmSlider.value = _saveSystem.settingsData._bgmVolume;
        sfxSlider.value = _saveSystem.settingsData._sfxVolume;
    }

    private void OnDisable()
    {
        _inputReader.UICancelEvent -= CancelBtn;
    }

    public void SetBGMVolume(float f)
    {
        _musicVolumeEventChannel.RaiseEvent(f);
        _onSettingsRequested.RaiseSaveAction(f, _saveSystem.settingsData._sfxVolume);
    }

    public void SetSFXVolume(float f)
    {
        _SFXVolumeEventChannel.RaiseEvent(f);
        _onSettingsRequested.RaiseSaveAction(_saveSystem.settingsData._bgmVolume, f);
    }

    public void CancelBtn()
    {
        OnCloseSettingsPanel.Invoke();
    }
}

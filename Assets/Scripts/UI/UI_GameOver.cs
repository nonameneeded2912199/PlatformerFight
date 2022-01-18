using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField]
    private Button _resumeButton = default;

    [SerializeField]
    private Button _backToMenuButton = default;

    public event UnityAction ResumedFromSave = default;
    public event UnityAction BackToMenu = default;

    private void OnEnable()
    {
        _resumeButton.onClick.AddListener(ResumedFromSave);
        _backToMenuButton.onClick.AddListener(BackToMenu);
    }

    private void OnDisable()
    {
        _resumeButton.onClick.RemoveAllListeners();
        _backToMenuButton.onClick.RemoveAllListeners();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UI_CharacterSelector : MonoBehaviour
{
    public UnityAction OnCloseCharacterSelectPanel;

    private int selectedCharacter;

    [SerializeField]
    private PlayableCharacterInfo[] playableCharacterInfos;

    [SerializeField]
    private VoidEventChannelSO _startNewGameEvent = default;

    [SerializeField]
    private InputReader _inputReader = default;

    [SerializeField]
    private GameStateSO gameStateSO = default;

    [SerializeField]
    private Image characterPortrait;

    [SerializeField]
    private TextMeshProUGUI characterName;

    [SerializeField]
    private TextMeshProUGUI characterStats;

    [SerializeField]
    private TextMeshProUGUI characterDescription;

    private void Awake()
    {
        selectedCharacter = 0;
        UpdateCharacterSelectUI();
    }

    private void OnEnable()
    {
        _inputReader.UICancelEvent += CancelCharacterSelect;
        selectedCharacter = 0;
        UpdateCharacterSelectUI();
    }

    private void OnDisable()
    {
        _inputReader.UICancelEvent -= CancelCharacterSelect;
    }

    private void CancelCharacterSelect()
    {
        OnCloseCharacterSelectPanel.Invoke();
    }

    private void UpdateCharacterSelectUI()
    {
        characterName.text = playableCharacterInfos[selectedCharacter].CharacterName;
        characterPortrait.sprite = playableCharacterInfos[selectedCharacter].CharacterSprite;
        string characterStats = " ";
        characterStats += "HP: " + playableCharacterInfos[selectedCharacter].CharacterStats.baseHP + "\n";
        characterStats += "AP: " + playableCharacterInfos[selectedCharacter].CharacterStats.baseAP + "  " 
            + "Recovery Rate: " + playableCharacterInfos[selectedCharacter].CharacterStats.apRecoveryRate.ToString() + "%" + "\n";
        characterStats += "ATK: " + playableCharacterInfos[selectedCharacter].CharacterStats.baseATK + "\n";
        characterStats += "DEF: " + playableCharacterInfos[selectedCharacter].CharacterStats.baseDEF + "\n";
        this.characterStats.text = characterStats;
        characterDescription.text = playableCharacterInfos[selectedCharacter].CharacterDescription;
    }

    public void OnNextButton_Click()
    {
        if (++selectedCharacter == playableCharacterInfos.Length)
            selectedCharacter = 0;
        UpdateCharacterSelectUI();
    }

    public void OnPrevButton_Click()
    {
        if (--selectedCharacter < 0)
            selectedCharacter = playableCharacterInfos.Length - 1;
        UpdateCharacterSelectUI();
    }

    public void OnSelectCharButton_Click()
    {
        gameStateSO.SelectCharacter(playableCharacterInfos[selectedCharacter]);
        _startNewGameEvent.RaiseEvent();
    }
}

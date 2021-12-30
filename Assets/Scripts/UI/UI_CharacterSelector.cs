using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UI_CharacterSelector : MonoBehaviour
{
    public UnityAction OnCloseCharacterSelectPanel;

    private int selectedCharacter;

    [SerializeField]
    private PlayableDatabaseSO _playableDatabaseSO = default;

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
        PlayableCharacterInfo currentPlayableInfo = _playableDatabaseSO.GetPlayableInfo(selectedCharacter);
        
        characterName.text = currentPlayableInfo.CharacterName;
        characterPortrait.sprite = currentPlayableInfo.CharacterSprite;

        StringBuilder sb = new StringBuilder("");

        sb.Append("HP: " + currentPlayableInfo.CharacterStats.baseHP + "\n");
        sb.Append("AP: " + currentPlayableInfo.CharacterStats.baseAP + "  "
            + "Recovery Rate: " + currentPlayableInfo.CharacterStats.apRecoveryRate.ToString() + "%" + "\n");
        sb.Append("ATK: " + currentPlayableInfo.CharacterStats.baseATK + "\n");
        sb.Append("DEF: " + currentPlayableInfo.CharacterStats.baseDEF + "\n");

        characterStats.text = sb.ToString();
        characterDescription.text = currentPlayableInfo.CharacterDescription;
    }

    public void OnNextButton_Click()
    {
        if (++selectedCharacter == _playableDatabaseSO.Amount)
            selectedCharacter = 0;
        UpdateCharacterSelectUI();
    }

    public void OnPrevButton_Click()
    {
        if (--selectedCharacter < 0)
            selectedCharacter = _playableDatabaseSO.Amount - 1;
        UpdateCharacterSelectUI();
    }

    public void OnSelectCharButton_Click()
    {
        gameStateSO.SelectCharacter(_playableDatabaseSO.GetPlayableInfo(selectedCharacter).CharacterID);
        _startNewGameEvent.RaiseEvent();
    }
}

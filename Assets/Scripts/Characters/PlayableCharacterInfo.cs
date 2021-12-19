using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayableInfoSO", menuName = "Characters/Playable Character Info")]
public class PlayableCharacterInfo : ScriptableObject
{
    [SerializeField]
    private int characterID;

    public int CharacterID
    {
        get => characterID;
    }

    [SerializeField]
    public GameObject playerGameObject;

    [SerializeField]
    private CharacterData_SO characterStats;

    public CharacterData_SO CharacterStats => characterStats;

    [SerializeField]
    private string characterName;

    public string CharacterName => characterName;

    [SerializeField]
    private string characterDescription;

    public string CharacterDescription => characterDescription;

    [SerializeField]
    private Sprite characterSprite;

    public Sprite CharacterSprite => characterSprite;
}

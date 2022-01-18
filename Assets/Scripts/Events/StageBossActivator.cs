using PlatformerFight.CharacterThings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBossActivator : MonoBehaviour
{
    [SerializeField]
    private GameStateSO gameState = default;

    [SerializeField]
    private DialogueEventChannelSO onOpenDialogue = default;

    [SerializeField]
    private Boss boss;

    [SerializeField]
    private bool isUniqueDialogue = false;

    [SerializeField]
    private TextAsset generalDialogue;

    [SerializeField]
    private List<CharacterDialogue> uniqueDialoguesList;
    private Dictionary<int, TextAsset> uniqueDialogues;

    public bool triggered = false;

    private void Awake()
    {
        uniqueDialogues = new Dictionary<int, TextAsset>(); 
    }

    private void Start()
    {
        foreach (CharacterDialogue cd in uniqueDialoguesList)
        {
            uniqueDialogues.Add(cd.characterID, cd.dialogue);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !triggered)
        {
            ActivateBoss();
            triggered = true;
        }
    }

    private void ActivateBoss()
    {
        if (isUniqueDialogue && uniqueDialogues.Count > 0)
        {
            TextAsset dialogue = GetUniqueDialogues(gameState.ChosenPlayerID);
            if (dialogue != null)
            {
                onOpenDialogue.RaiseEvent(dialogue, new Action(() => boss.Activate()));
            }    
            else
            {
                Debug.Log("Failed dialogue");
                boss.Activate();
            }    
        }   
        else
        {
            if (generalDialogue != null)
            {
                onOpenDialogue.RaiseEvent(generalDialogue, new Action(() => boss.Activate()));
            }   
            else
            {
                Debug.Log("Failed dialogue");
                boss.Activate();
            }    
        }    
    }

    private TextAsset GetUniqueDialogues(int id)
    {
        TextAsset dialogue = null;

        Debug.Log(id);
        Debug.Log(uniqueDialogues.ContainsKey(id));

        if (uniqueDialogues.ContainsKey(id))
            dialogue = uniqueDialogues[id];

        return dialogue;
    }
}

[Serializable]
public struct CharacterDialogue
{
    public int characterID;
    public TextAsset dialogue;
}

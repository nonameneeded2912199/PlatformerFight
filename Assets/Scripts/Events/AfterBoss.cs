using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterBoss : MonoBehaviour
{
    [SerializeField]
    private GameStateSO gameState = default;

    [SerializeField]
    private VoidEventChannelSO onActivateDialogue = default;

    [SerializeField]
    private DialogueEventChannelSO onOpenDialogue = default;

    [SerializeField]
    private VoidEventChannelSO onAfterDialogue = default;

    [SerializeField]
    private bool isUniqueDialogue = false;

    [SerializeField]
    private TextAsset generalDialogue;

    [SerializeField]
    private List<CharacterDialogue> uniqueDialoguesList;
    private Dictionary<int, TextAsset> uniqueDialogues;

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

    private void OnEnable()
    {
        onActivateDialogue.OnEventRaised += ActivateDialogue;
    }

    private void OnDisable()
    {
        onActivateDialogue.OnEventRaised -= ActivateDialogue;
    }

    private void ActivateDialogue()
    {
        if (isUniqueDialogue && uniqueDialogues.Count > 0)
        {
            TextAsset dialogue = GetUniqueDialogues(gameState.ChosenPlayerID);
            if (dialogue != null)
            {
                onOpenDialogue.RaiseEvent(dialogue, new Action(() => onAfterDialogue.RaiseEvent()));
            }
            else
            {
                Debug.Log("Failed dialogue");
                onAfterDialogue.RaiseEvent();
            }
        }
        else
        {
            if (generalDialogue != null)
            {
                onOpenDialogue.RaiseEvent(generalDialogue, new Action(() => onAfterDialogue.RaiseEvent()));
            }
            else
            {
                Debug.Log("Failed dialogue");
                onAfterDialogue.RaiseEvent();
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

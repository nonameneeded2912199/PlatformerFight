using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Ink.Runtime;

public class UI_DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject advanceButton;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [SerializeField] 
    private float typingSpeed = 0.04f;

    [SerializeField]
    private float defaultDialogueTextSize = 53f;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; } = false;

    private bool canContinueToNextLine = false;

    private bool advanceButtonPressed = false;

    private Coroutine displayLineCoroutine;

    private Action afterDialogueAction;

    [SerializeField]
    private InputReader inputReader = default;

    private void OnEnable()
    {
        inputReader.AdvanceDialogueEvent += ContinueStory;
    }

    private void OnDisable()
    {
        inputReader.AdvanceDialogueEvent -= ContinueStory;
    }

    public void EnterDialogueMode(TextAsset textAsset, Action afterDialogue)
    {
        currentStory = new Story(textAsset.text);
        afterDialogueAction = afterDialogue;
        dialogueIsPlaying = true;
        //dialoguePanel.SetActive(true);

        ContinueStory();
    }

    public void ContinueStory()
    {
        advanceButtonPressed = true;
        if (currentStory.canContinue)
        {
            advanceButtonPressed = false;

            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            dialogueText.fontSize = defaultDialogueTextSize;
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            HandleTags(currentStory.currentTags);
        }    
        else
        {
            StartCoroutine(ExitDialogue());
        }
    }

    private IEnumerator ExitDialogue()
    {
        yield return new WaitForSeconds(0.2f);


        afterDialogueAction.Invoke();
        inputReader.EnableGameplayInput();
        dialogueIsPlaying = false;
        gameObject.SetActive(false);
        dialogueText.text = "";
        afterDialogueAction = null;
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;
        foreach (char letter in line.ToCharArray())
        {
            // if the submit button is pressed, finish up displaying the line right away
            if (advanceButtonPressed)
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            // check for rich text tag, if found, add it without waiting
            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            // if not rich text, add the next letter and wait a small time
            else
            {
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        canContinueToNextLine = true;
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // handle the tag
            switch (tagKey)
            {
                case "speaker":
                    nameText.text = tagValue;
                    break;
                case "size":
                    dialogueText.fontSize = float.Parse(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }
}

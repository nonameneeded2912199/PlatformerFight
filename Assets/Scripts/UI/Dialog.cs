using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour, IDialog
{
    #region Dialog box's data structure

    private class ButtonData
    {
        public string Text { get; set; }

        public Action Action { get; set; }
    }

    private class DialogData
    {
        public string Header { get; set; }

        public string BodyText { get; set; }

        public List<ButtonData> Buttons { get; private set; }

        public DialogData()
        {
            Buttons = new List<ButtonData>();
        }
    }

    #endregion

    private DialogData dialogData;

    [SerializeField]
    private TextMeshProUGUI headerText;

    [SerializeField]
    private TextMeshProUGUI bodyText;

    [SerializeField]
    private Transform buttonHolder;

    [SerializeField]
    private GameObject buttonPrefab;

    [SerializeField]
    private GameObject dialogBackground;

    [SerializeField]
    private GameObject dialogBox;

    [SerializeField]
    private DialogEventChannelSO _onDialogRequested = default;

    private void OnEnable()
    {
        _onDialogRequested.OnDialogRequested += GetDialog;
    }

    private void OnDisable()
    {
        _onDialogRequested.OnDialogRequested -= GetDialog;
    }

    void Start()
    {
        dialogData = new DialogData();

        dialogBox.SetActive(false);
    }  

    public IDialog AddButton(string text, Action action)
    {
        dialogData.Buttons.Add(new ButtonData() { Text = text, Action = action });
        return this;
    }

    public IDialog AddCancelButton(string text)
    {
        dialogData.Buttons.Add(new ButtonData() { Text = text, Action = () => { } });
        return this;
    }

    public IDialog SetText(string text)
    {
        dialogData.BodyText = text;
        return this;
    }

    public IDialog SetTitle(string text)
    {
        dialogData.Header = text;
        return this;
    }

    public IDialog Show()
    {
        // Setting the text
        headerText.text = dialogData.Header;
        bodyText.text = dialogData.BodyText;

        // Remove the existing button
        foreach (Transform t in buttonHolder)
        {
            Destroy(t.gameObject);
        }

        // Now add new ones
        foreach (var b in dialogData.Buttons)
        {
            CreateButton(b);
        }

        // Show the dialog box
        dialogBackground.SetActive(true);
        dialogBox.SetActive(true);

        // Set up the dialog system for the new dialog
        dialogData = new DialogData();

        return this;
    }

    private void CreateButton(ButtonData buttonData)
    {
        GameObject buttonClone = Instantiate(buttonPrefab, buttonHolder);
        buttonClone.transform.localScale = Vector3.one;
        var txt = buttonClone.GetComponentInChildren<TextMeshProUGUI>();
        var button = buttonClone.GetComponentInChildren<Button>();

        Action fireAndClose = () =>
        {
            //buttonData.Action();
            dialogBackground.SetActive(false);
            dialogBox.SetActive(false);
            buttonData.Action();
        };

        txt.text = buttonData.Text;
        button.onClick.AddListener(new UnityEngine.Events.UnityAction(fireAndClose));
    }

    private IDialog GetDialog()
    {
        return this;
    }
}

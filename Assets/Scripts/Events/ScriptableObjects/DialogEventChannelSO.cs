using UnityEngine;

[CreateAssetMenu(menuName = "Events/Dialog Event Channel")]
public class DialogEventChannelSO : ScriptableObject
{
    public delegate IDialog RequestDialogAction();

    public RequestDialogAction OnDialogRequested;

    public IDialog RaiseSaveAction()
    {
        IDialog dialog = null;
        if (OnDialogRequested != null)
        {
            dialog = OnDialogRequested.Invoke();
        }
        else
        {
            Debug.LogWarning("An Save play event was requested but nobody picked it up. ");
        }

        return dialog;
    }
}

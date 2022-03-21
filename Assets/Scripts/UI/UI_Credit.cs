using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_Credit : MonoBehaviour
{
    [SerializeField]
    private InputReader _inputReader = default;

    public UnityAction OnCloseCredits;

    private void OnEnable()
    {
        _inputReader.UICancelEvent += CancelBtn;
    }

    private void OnDisable()
    {
        _inputReader.UICancelEvent -= CancelBtn;
    }

    public void CancelBtn()
    {
        OnCloseCredits.Invoke();
    }
}

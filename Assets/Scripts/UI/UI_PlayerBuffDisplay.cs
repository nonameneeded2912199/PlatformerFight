using PlatformerFight.Buffs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerBuffDisplay : MonoBehaviour
{
    [SerializeField]
    private BuffEventChannelSO _onUpdateBuffDisplay;

    [SerializeField]
    private Transform buffsContainer;

    private void OnEnable()
    {
        _onUpdateBuffDisplay.OnRaiseEvent += AddBuffImage;
    }

    private void OnDisable()
    {
        _onUpdateBuffDisplay.OnRaiseEvent -= AddBuffImage;
    }

    private void AddBuffImage(BaseBuff buff)
    {
        buff.BuffIconPrefab.transform.SetParent(buffsContainer);
        buff.BuffIconPrefab.GetComponent<RectTransform>().pivot = Vector2.zero;
    }
}

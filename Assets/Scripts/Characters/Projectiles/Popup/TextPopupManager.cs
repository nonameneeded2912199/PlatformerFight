using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopupManager : MonoBehaviour
{
    [Header("Bullets pool")]
    [SerializeField]
    private TextPopupPoolSO popupPool = default;
    [SerializeField]
    private int initialSize = 10;

    [Header("Listening on channels")]
    [Tooltip("The DamagePopupManager listens to this event, fired by objects in any scene, to spawn Bullet")]
    [SerializeField]
    private TextPopupEventChannelSO popupEventChannel = default;

    private void Awake()
    {
        popupPool.Prewarm(initialSize);
        popupPool.SetParent(transform);
    }

    private void OnEnable()
    {
        popupEventChannel.OnPopupSpawnRequested += SpawnPopup;
        popupEventChannel.OnPopupBackToPoolRequested += RetrievePopup;
    }

    private void OnDestroy()
    {
        popupEventChannel.OnPopupSpawnRequested -= SpawnPopup;
        popupEventChannel.OnPopupBackToPoolRequested -= RetrievePopup;
    }

    private TextPopup SpawnPopup(string content, Vector2 position, float destroyTime = 3f, DamageType damageType = DamageType.NormalDamage)
    {
        TextPopup popup = popupPool.Request();
        popup.SetPopup(content, position, destroyTime, damageType);   
        return popup;
    }

    private bool RetrievePopup(TextPopup popup)
    {
        try
        {
            popupPool.Return(popup);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        return false;
    }
}

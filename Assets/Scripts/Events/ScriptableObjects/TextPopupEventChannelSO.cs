using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Text popup Event Channel")]
public class TextPopupEventChannelSO : DescriptionBaseSO
{
    public delegate TextPopup TextPopupSpawnAction(string content, Vector2 position, float destroyTime = 3f, DamageType damageType = DamageType.NormalDamage);

    public delegate bool TextPopupBackToPoolAction(TextPopup textPopup);

    public TextPopupSpawnAction OnPopupSpawnRequested;
    public TextPopupBackToPoolAction OnPopupBackToPoolRequested;

    public TextPopup RaiseTextPopupEvent(string content, Vector2 position, float destroyTime = 3f, DamageType damageType = DamageType.NormalDamage)
    {
        TextPopup popup = default;

        if (OnPopupSpawnRequested != null)
        {
            popup = OnPopupSpawnRequested.Invoke(content, position, destroyTime, damageType);
        }
        else
        {
            Debug.LogWarning("An TextPopup play event was requested  for " + popup.name + ", but nobody picked it up. " +
                "Check why there is no TextPopupManager already loaded, " +
                "and make sure it's listening on this TextPopup Event channel.");
        }

        return popup;
    }

    public bool RaiseReturnTextPopupEvent(TextPopup popup)
    {
        bool requestSucceed = false;

        if (OnPopupBackToPoolRequested != null)
        {
            requestSucceed = OnPopupBackToPoolRequested.Invoke(popup);
        }
        else
        {
            Debug.LogWarning("An TextPopup play event was requested  for " + popup.name + ", but nobody picked it up. " +
                "Check why there is no TextPopupManager already loaded, " +
                "and make sure it's listening on this TextPopup Event channel.");
        }

        return requestSucceed;
    }
}

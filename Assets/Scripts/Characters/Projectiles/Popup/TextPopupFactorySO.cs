using PlatformerFight.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTextPopupFactory", menuName = "Factory/TextPopup Factory")]
public class TextPopupFactorySO : FactorySO<TextPopup>
{
    public TextPopup popupPrefab = default;

    public override TextPopup Create()
    {
        return Instantiate(popupPrefab);
    }
}

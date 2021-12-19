using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusBar : MonoBehaviour
{
    public Image barImage;
    public TextMeshProUGUI textStatus;
    public Image barEffectImage;

    [SerializeField]
    private float transitionLerp = 0.005f;

    public void UpdateBar(float currentValue, float maxValue)
    {
        barImage.fillAmount = (float)currentValue / (float)maxValue;
        if (textStatus != null)
            textStatus.text = (int)currentValue + " / " + (int)maxValue;

        if (barEffectImage.fillAmount > barImage.fillAmount)
        {
            barEffectImage.fillAmount -= transitionLerp;
        }
        else
        {
            barEffectImage.fillAmount = barImage.fillAmount;
        }
    }
}
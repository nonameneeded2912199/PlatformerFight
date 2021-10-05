using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour, TouchOutsideInterface
{
    [SerializeField]
    Slider slider;

    private void Start()
    {
        slider.maxValue = 1;
        slider.minValue = 0;
        slider.value = PlayerPrefs.GetFloat("Volume");
    }

    public void SetBGMVolume(float f)
    {
        BGMManager.Instance.bgmSource.volume = f;
        PlayerPrefs.SetFloat("Volume", f);
    }

    public void CancelBtn()
    {
        gameObject.SetActive(false);
    }

    public void TouchOutside()
    {
        CancelBtn();
    }
}

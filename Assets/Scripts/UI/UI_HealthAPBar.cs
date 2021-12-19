using PlatformerFight.CharacterThings;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HealthAPBar : MonoBehaviour
{
    [Header("Event channel")]
    [SerializeField]
    private CharacterStatsEventChannelSO _onPlayerStatUpdate;

    [Header("HP Asset")]
    [SerializeField]
    private StatusBar hpBar;

    [Space]
    [Header("HP Asset")]
    [SerializeField]
    private StatusBar apBar;

    private void OnEnable()
    {
        _onPlayerStatUpdate.OnEventRaised += UpdateStats;
    }

    private void OnDisable()
    {
        _onPlayerStatUpdate.OnEventRaised -= UpdateStats;
    }

    private void UpdateStats(CharacterStats playerStats)
    {
        hpBar.UpdateBar(playerStats.CurrentHP, playerStats.MaxHP);
        apBar.UpdateBar(playerStats.CurrentAP, playerStats.MaxAP);
    }
}

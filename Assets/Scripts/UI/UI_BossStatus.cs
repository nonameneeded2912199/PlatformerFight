using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UI_BossStatus : MonoBehaviour
{
    [Header("Event channel")]
    [SerializeField]
    private CharacterStatsEventChannelSO _onBossStatUpdate;

    [SerializeField]
    private BossPhaseEventChannelSO _onBossStatusStart;

    [SerializeField]
    private VoidEventChannelSO _onBossStatusEnd;

    [SerializeField, Tooltip("Boss's name")]
    private TextMeshProUGUI bossName;

    [SerializeField, Tooltip("Attack Phase's name")]
    private TextMeshProUGUI phaseName;

    [SerializeField, Tooltip("Boss's health")]
    private StatusBar hpBar;

    [SerializeField, Tooltip("Boss's time")]
    private TextMeshProUGUI phaseTime;

    private bool isBossBattle;

    private void Awake()
    {
        bossName.gameObject.SetActive(false);
        phaseName.gameObject.SetActive(false);
        hpBar.gameObject.SetActive(false);
        phaseTime.gameObject.SetActive(false);
        GetComponent<CanvasGroup>().DOFade(0, 0.1f);
    }

    private void StartPhase(BossPhaseDataSO phase)
    {
        isBossBattle = true;

        if (phase.enemyName != "" && phase.enemyName != null)
        {
            bossName.gameObject.SetActive(true);
            bossName.text = phase.enemyName;
        }

        if (phase.phaseType == BossPhaseType.SpellCardAttack)
        {
            if (phase.phaseName != "" && phase.phaseName != null)
            {
                phaseName.gameObject.SetActive(true);
                phaseName.text = phase.phaseName;
            }

            if (phase.phaseBonusTime > 0)
            {
                phaseTime.gameObject.SetActive(true);
                phaseTime.text = phase.phaseBonusTime.ToString();
            }
        }

        hpBar.gameObject.SetActive(true);
        GetComponent<CanvasGroup>().DOFade(1, 0.5f);
    }

    private void EndPhase()
    {
        isBossBattle = false;

        GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() =>
        {
            bossName.gameObject.SetActive(false);
            phaseName.gameObject.SetActive(false);
            hpBar.gameObject.SetActive(false);
            phaseTime.gameObject.SetActive(false);
        });
    }

    private void OnEnable()
    {
        _onBossStatUpdate.OnEventRaised += UpdateBossHP;
        _onBossStatusStart.OnEventRaised += StartPhase;
        _onBossStatusEnd.OnEventRaised += EndPhase;
    }

    private void OnDisable()
    {
        _onBossStatUpdate.OnEventRaised -= UpdateBossHP;
        _onBossStatusStart.OnEventRaised -= StartPhase;
        _onBossStatusEnd.OnEventRaised -= EndPhase;
    }

    private void UpdateBossHP(CharacterStats bossStats)
    {
        if (isBossBattle && hpBar.gameObject.activeInHierarchy)
            hpBar.UpdateBar(bossStats.CurrentHP, bossStats.MaxHP);
    }

}

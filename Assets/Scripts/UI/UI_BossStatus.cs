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
    private CharacterStatsEventChannelSO _onBossStatUpdate = default;

    [SerializeField]
    private BossPhaseEventChannelSO _onBossStatusStart = default;

    [SerializeField]
    private VoidEventChannelSO _onBossStatusEnd = default;

    [SerializeField]
    private FloatEventChannelSO _onBossTimerUpdate = default;

    [SerializeField]
    private PhaseResultEventChannelSO _onSpellPhaseCompleted = default;


    [Space]
    [Header("Attributes")]
    [SerializeField, Tooltip("Boss's name")]
    private TextMeshProUGUI bossName;

    [SerializeField, Tooltip("Attack Phase's name")]
    private TextMeshProUGUI phaseName;

    [SerializeField, Tooltip("Boss's health")]
    private StatusBar hpBar;

    [SerializeField, Tooltip("Time title")]
    private TextMeshProUGUI timeTitle;

    [SerializeField, Tooltip("Boss's time")]
    private TextMeshProUGUI phaseTime;

    [SerializeField, Tooltip("Bonus Title")]
    private TextMeshProUGUI bonusTitle;

    [SerializeField, Tooltip("Bonus Time")]
    private TextMeshProUGUI bonusScore;

    [SerializeField, Tooltip("Bonus Fail")]
    private TextMeshProUGUI bonusFail;

    private bool isBossBattle;

    private void Awake()
    {
        bossName.gameObject.SetActive(false);
        phaseName.gameObject.SetActive(false);
        hpBar.gameObject.SetActive(false);
        phaseTime.gameObject.SetActive(false);
        timeTitle.gameObject.SetActive(false);
        bonusScore.gameObject.SetActive(false);
        bonusTitle.gameObject.SetActive(false);
        bonusFail.gameObject.SetActive(false);
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
                timeTitle.gameObject.SetActive(true);
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
            timeTitle.gameObject.SetActive(false);
        });
    }

    private void OnEnable()
    {
        _onBossStatUpdate.OnEventRaised += UpdateBossHP;
        _onBossStatusStart.OnEventRaised += StartPhase;
        _onBossStatusEnd.OnEventRaised += EndPhase;
        _onBossTimerUpdate.OnEventRaised += UpdateBossTimer;
        _onSpellPhaseCompleted.OnEventRaised += CompleteSpellPhase;
    }

    private void OnDisable()
    {
        _onBossStatUpdate.OnEventRaised -= UpdateBossHP;
        _onBossStatusStart.OnEventRaised -= StartPhase;
        _onBossStatusEnd.OnEventRaised -= EndPhase;
        _onBossTimerUpdate.OnEventRaised -= UpdateBossTimer;
        _onSpellPhaseCompleted.OnEventRaised -= CompleteSpellPhase;
    }

    private void UpdateBossHP(CharacterStats bossStats)
    {
        if (isBossBattle && hpBar.gameObject.activeInHierarchy)
            hpBar.UpdateBar(bossStats.CurrentHP, bossStats.MaxHP);
    }

    private void UpdateBossTimer(float time)
    {
        if (isBossBattle && phaseTime.gameObject.activeInHierarchy)
        {
            phaseTime.text = time.ToString("00.00");
        }    
    }  
    
    private void CompleteSpellPhase(bool success, long score)
    {
        if (success)
        {
            bonusScore.gameObject.SetActive(true);
            bonusTitle.gameObject.SetActive(true);
            bonusFail.gameObject.SetActive(false);

            bonusScore.text = score.ToString();
        }
        else
        {
            bonusScore.gameObject.SetActive(false);
            bonusTitle.gameObject.SetActive(false);
            bonusFail.gameObject.SetActive(true);
        }

        Invoke("HideResult", 3f);
    }

    private void HideResult()
    {
        bonusScore.gameObject.SetActive(false);
        bonusTitle.gameObject.SetActive(false);
        bonusFail.gameObject.SetActive(false);
    }

}

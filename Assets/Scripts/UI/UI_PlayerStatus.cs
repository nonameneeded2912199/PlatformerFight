using System.Collections;
using PlatformerFight.CharacterThings;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerStatus : MonoBehaviour
{
    [Header("Event channel")]
    [SerializeField]
    private CharacterStatsEventChannelSO _onPlayerStatUpdate;

    [SerializeField]
    private VoidEventChannelSO _onScoreLives = default;

    [SerializeField]
    private IntEventChannelSO _onUpdateLives = default;

    [SerializeField]
    private LongEventChannelSO _onUpdateScore = default;

    private Coroutine updateScoreCoroutine;

    [Header("HP Asset")]
    [SerializeField]
    private StatusBar hpBar;

    [Space]
    [Header("HP Asset")]
    [SerializeField]
    private StatusBar apBar;

    [Space]
    [Header("Lives")]
    [SerializeField]
    private TextMeshProUGUI livesText;

    [Space]
    [Header("Lives")]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [Header("Game state SO")]
    [SerializeField]
    private GameStateSO gameStateSO = default;

    [SerializeField]
    private float incrementTime = 4;


    private void OnEnable()
    {
        livesText.text = "Lives: x" + gameStateSO.LifeCount;
        scoreText.text = "" + gameStateSO.Score;

        _onPlayerStatUpdate.OnEventRaised += UpdateStats;
        _onUpdateLives.OnEventRaised += UpdateLives;
        _onUpdateScore.OnEventRaised += UpdateScore;
    }

    private void OnDisable()
    {
        _onPlayerStatUpdate.OnEventRaised -= UpdateStats;
        _onUpdateLives.OnEventRaised -= UpdateLives;
        _onUpdateScore.OnEventRaised -= UpdateScore;
    }

    private void UpdateLives(int additionalLives)
    {
        gameStateSO.LifeCount += additionalLives;

        livesText.text = "Lives: x" + gameStateSO.LifeCount;
    }

    private void UpdateScore(long additionalScore)
    {
        long oldScore = gameStateSO.Score;

        gameStateSO.Score += additionalScore;

        if (updateScoreCoroutine != null)
            StopCoroutine(updateScoreCoroutine);

        updateScoreCoroutine = StartCoroutine(IncrementScore(oldScore, gameStateSO.Score));

        _onScoreLives.RaiseEvent();
    }    

    private IEnumerator IncrementScore(long originalScore, long afterScore)
    {
        float time = 0;

        while (time < incrementTime)
        {
            yield return null;

            time += Time.deltaTime;

            float factor = time / incrementTime;

            scoreText.text = ((long)Mathf.Lerp(originalScore, afterScore, factor)).ToString();
        }

        yield break;
    }    

    private void UpdateStats(CharacterStats playerStats)
    {
        hpBar.UpdateBar(playerStats.CurrentHP, playerStats.MaxHP);
        apBar.UpdateBar(playerStats.CurrentAP, playerStats.MaxAP);
    }
}

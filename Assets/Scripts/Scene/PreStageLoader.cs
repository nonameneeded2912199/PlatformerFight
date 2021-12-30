using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PreStageLoader : MonoBehaviour
{
    [Header("UI Parts")]
    [SerializeField]
    private GameObject mainUI;

    [SerializeField]
    private TextMeshProUGUI livesText;

    [SerializeField]
    private TextMeshProUGUI stageText;

    [SerializeField]
    private TextMeshProUGUI difficultyText;

    [SerializeField]
    private TextMeshProUGUI loadingText;

    [Space]

    [Header("Event Channel")]
    [SerializeField]
    private GameStateSO _gameStateSO = default;
    [SerializeField] 
    private LoadEventChannelSO _loadStage = default;
    [SerializeField]
    private InputReader _inputReader = default;

    private void Awake()
    {
        _inputReader.DisableAllInput();
        livesText.text = "Lives: " + _gameStateSO.LifeCount;
        stageText.text = _gameStateSO.CurrentStage.stageName;
        difficultyText.text = "Difficulty: " + _gameStateSO.CurrentDifficulty.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        mainUI.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        StartCoroutine(WaitBeforeLoad());
    }

    // Update is called once per frame
    void Update()
    {
        if (loadingText.GetComponent<CanvasGroup>().alpha == 0)
        {
            loadingText.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        }    
        else if (loadingText.GetComponent<CanvasGroup>().alpha == 1)
        {
            loadingText.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        }
    }

    void FadeInLoading()
    {
        loadingText.GetComponent<CanvasGroup>().DOFade(1, 0.5f).OnComplete(
            () =>
            {
                FadeOutLoading();
            }
            );
    }    

    void FadeOutLoading()
    {
        loadingText.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(
            () =>
            {
                FadeInLoading();
            }
            );
    }

    private IEnumerator WaitBeforeLoad()
    {
        yield return new WaitForSeconds(5f);

        mainUI.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() =>
        {
            _loadStage.RaiseEvent(_gameStateSO.CurrentStage, true, true);
        });
    }    
}

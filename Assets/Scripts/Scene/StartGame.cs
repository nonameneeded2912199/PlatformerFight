using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private GameSceneSO stageToLoad;

    [SerializeField]
    private bool showLoadScreen = default;

    [Header("Broadcasting on")]
    [SerializeField] private LoadEventChannelSO loadLocation = default;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO onNewGameButton = default;
    [SerializeField] private VoidEventChannelSO onContinueButton = default;

    // Start is called before the first frame update
    void Start()
    {
        onNewGameButton.OnEventRaised += StartNewGame;
    }

    // Update is called once per frame
    void OnDestroy()
    {
        onNewGameButton.OnEventRaised -= StartNewGame;
    }

    private void StartNewGame()
    {
        loadLocation.RaiseEvent(stageToLoad, true, true);
    }

    private void ContinueGame()
    {

    }

    private IEnumerator LoadSaveGame()
    {
        yield return null;
    }
}

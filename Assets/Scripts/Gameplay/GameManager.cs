using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameStateSO _gameState = default;

    void Awake()
    {
        
    }

    void Update()
    {
    }

    public void StartGame()
    {
        _gameState.UpdateGameState(GameState.Gameplay);
    }    
}

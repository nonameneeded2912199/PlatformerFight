using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelect : MonoBehaviour, TouchOutsideInterface
{    
    public void EasyBtn()
    {
        GameManager.Instance.currentGameDifficulty = GameDifficulty.EASY;
        ScenesManager.Instance.StartGame();
    }   
    
    public void NormalBtn()
    {
        GameManager.Instance.currentGameDifficulty = GameDifficulty.NORMAL;
        ScenesManager.Instance.StartGame();
    }    

    public void HardBtn()
    {
        GameManager.Instance.currentGameDifficulty = GameDifficulty.HARD;
        ScenesManager.Instance.StartGame();
    }   
    
    public void LunaticBtn()
    {
        GameManager.Instance.currentGameDifficulty = GameDifficulty.LUNATIC;
        ScenesManager.Instance.StartGame();
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

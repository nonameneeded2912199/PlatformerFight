using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseScreen;

    void Update()
    {
        /*#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseScreen.activeInHierarchy)
                PauseButton();
            else
                Continue();
        }    
        #endif*/
    }

    public void PauseButton()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void ToMenu()
    {
        Time.timeScale = 1;
    }    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneScript : MonoBehaviour
{
    private bool isSaveDataExist;

    private PlayerData playerData;

    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private GameObject difficultySelect;

    [SerializeField]
    private GameObject settingsPanel;

    private void Awake()
    {
        if (!SaveManager.IsSaveDataAlreadyLoaded)
        {
            playerData = SaveManager.LoadGame();
            if (playerData != null)
            {
                PlayerPrefs.SetString(PlayerData.CHECKPOINT_NAME, playerData.checkPointName);

                PlayerPrefs.SetInt(PlayerData.SAVED_DIFFICULTY, playerData.difficulty);
                PlayerPrefs.SetString(PlayerData.SCENE_SAVED, playerData.sceneName);

                continueButton.onClick.AddListener(Continue);
            }
            else
            {
                continueButton.interactable = false;
            }    
        }
    }

    void Start()
    {
        //BGMManager.Instance.PlayBGM("Title");
    }

    public void StartGame()
    {
        if (playerData != null)
        {
            Dialog.Instance.SetTitle("Save data exists.")
                           .SetText("Saved data exists. Are you sure you want to start a new game?" + " \n " + "This will delete your previously saved data.")
                           .AddButton("Yes", () => { difficultySelect.SetActive(true); })
                           .AddCancelButton("No")
                           .Show();
        }
        else
        {
            difficultySelect.SetActive(true);
        }
    }   
    
    public void Continue()
    {
        if (playerData != null)
        {
            string stageNumber = playerData.sceneName.Split('-')[0];
            string info = "Player Progress: " + stageNumber + " \n " + "Difficulty: " + 
                (GameDifficulty)playerData.difficulty + " \n " + "Do you want to continue ?";

            Dialog.Instance.SetTitle("Your existing save data.")
                           .SetText(info)
                           .AddButton("Yes", () => 
                           {                              
                               SaveManager.CheckPointName = PlayerPrefs.GetString(PlayerData.CHECKPOINT_NAME);
                               SaveManager.SceneName = PlayerPrefs.GetString(PlayerData.SCENE_SAVED);
                               GameManager.Instance.currentGameDifficulty = (GameDifficulty)PlayerPrefs.GetInt(PlayerData.SAVED_DIFFICULTY);

                               ScenesManager.Instance.RestartScene();
                           })
                           .AddCancelButton("No")
                           .Show();
        }    
    }   

    public void Setting()
    {
        settingsPanel.SetActive(true);
    }    
    
    public void QuitGame()
    {
        Application.Quit();
    }    
}

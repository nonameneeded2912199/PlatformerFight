using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : Singleton<ScenesManager>
{
    //public static ScenesManager Instance { get; private set; }

    [SerializeField]
    private static readonly string SHARED_SCENE_NAME = "SharedScene";

    private CanvasGroup fadingGroup;
    public bool IsSceneReadyToBeLoaded { get; set; }

    [SerializeField]
    private float maxFadingTime;
    private float fadingTimer;

    void Awake()
    {
        /*base.Awake();
        DontDestroyOnLoad(gameObject);*/

        fadingGroup = GetComponentInChildren<CanvasGroup>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (!scene.name.Equals(SHARED_SCENE_NAME))
        {
            SceneManager.SetActiveScene(scene);
            switch (scene.name)
            {
                case "TitleScreen":
                    BGMManager.Instance.PlayBGM("Title");
                    break;
                case "Stage1-1":
                case "Stage1-2":
                case "Stage1-Preboss":
                    BGMManager.Instance.PlayBGM("Stage1");
                    break;
                case "Stage2-1":
                    BGMManager.Instance.PlayBGM("Stage2");
                    break;
            }
        }

        if (loadMode == LoadSceneMode.Single || (int)loadMode == 4)
        {
            StartCoroutine(FadingCoroutine(false));           
        }    
    }

    public void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            gameObject.SetActive(true);

            if (loadSceneMode == LoadSceneMode.Single)
            {
                StartCoroutine(FadingCoroutine(true));
            }    
            else
            {
                IsSceneReadyToBeLoaded = true;
            }    
            // Start Loading scene in another coroutine
            StartCoroutine(LoadSceneCoroutine(sceneName, loadSceneMode));
        }
    }

    public void LoadScene(string sceneName, LoadSceneMode loadSceneMode, float time)
    {
        gameObject.SetActive(true);

        StartCoroutine(WaitBeforeLoadingScene(sceneName, loadSceneMode, time));
    }

    public bool CheckIfSceneIsLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName && scene.isLoaded)
                return true;
        }
        return false;
    }

    private IEnumerator WaitBeforeLoadingScene(string sceneName, LoadSceneMode loadSceneMode, float time)
    {
        yield return new WaitForSeconds(time);

        LoadScene(sceneName, loadSceneMode);
    }    

    private IEnumerator LoadSceneCoroutine(string sceneName, LoadSceneMode loadSceneMode)
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        asyncOperation.allowSceneActivation = false;
        IsSceneReadyToBeLoaded = false;
        Debug.Log("Progress :" + asyncOperation.progress);
        while (!asyncOperation.isDone)
        {
            //Debug.Log("Current Progress = " + asyncOperation.progress * 100 + "%");

            if (asyncOperation.progress >= .9f)
            {
                //print("The Scene is ready!");
                if (IsSceneReadyToBeLoaded)
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void StartGame()
    {
        gameObject.SetActive(true);
        SaveManager.CheckPointName = "Checkpoint_1-1";
        StartCoroutine(RestartSceneCoroutine("Stage1-1"));
    }  

    public void RestartScene()
    {
        gameObject.SetActive(true);
        StartCoroutine(RestartSceneCoroutine(SaveManager.SceneName));
    }

    private IEnumerator FadingCoroutine(bool isFading)
    {
        // Reset timer
        fadingTimer = 0f;

        // Fading
        while (fadingTimer < maxFadingTime)
        {
            fadingTimer += Time.deltaTime;

            // Set the alpha paramter of canvas group's color
            if (isFading)
            {
                fadingGroup.alpha = fadingTimer / maxFadingTime;
            }
            else
            {
                fadingGroup.alpha = 1 - fadingTimer / maxFadingTime;
            }

            yield return fadingGroup.alpha;
        }

        if (isFading)
            IsSceneReadyToBeLoaded = true;
        else
            gameObject.SetActive(false);
    }    

    private IEnumerator RestartSceneCoroutine(string sceneName)
    {
        yield return null;

        //Scene scene = SceneManager.GetSceneByName(sceneName);
        //Debug.Log(scene.name);

        // Set this Flag to false. We only need to active the scene when all disired tasks are done.
        IsSceneReadyToBeLoaded = false;

        // Load Shared_Scene first
        AsyncOperation asyncSharedScene = SceneManager.LoadSceneAsync(SHARED_SCENE_NAME, LoadSceneMode.Single);
        StartCoroutine(FadingCoroutine(true));
        // Don't let the SharedScene activate, we want it to finish fading first
        asyncSharedScene.allowSceneActivation = false;

        // Additively re-load the current active scene
        AsyncOperation asyncCurrentScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Don't let the CurrentScene activate, since it has references to SharedScene
        // Must let it active after SharedScene is Loaded and Activated

        asyncCurrentScene.allowSceneActivation = false;
        asyncCurrentScene.completed += OnCompletedReloadCurrentScene;

        while (!asyncSharedScene.isDone)
        {
            if (asyncSharedScene.progress >= .9f)
            {
                //print("The Scene is ready!");
                //yield return new WaitForSeconds(5);

                if (IsSceneReadyToBeLoaded)
                {
                    //print("OK, Fading done! Active it!");
                    asyncSharedScene.allowSceneActivation = true;
                    asyncCurrentScene.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    private void OnCompletedReloadCurrentScene(AsyncOperation obj)
    {
        print("HIDE THE BLACK FOREGROUND DUDE");
        GameManager.Instance.RestartLevelFinished();
    }

    public void UnloadScene(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
            SceneManager.UnloadSceneAsync(sceneName);
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

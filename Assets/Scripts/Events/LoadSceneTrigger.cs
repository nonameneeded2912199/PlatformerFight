using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;

    [SerializeField]
    private string[] sceneToUnLoad;

    public bool triggered = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!triggered)
            {
                if (sceneToLoad != null && !sceneToLoad.Equals("") 
                    && !ScenesManager.Instance.CheckIfSceneIsLoaded(sceneToLoad))
                {
                    ScenesManager.Instance.LoadScene(sceneToLoad, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                }

                if (sceneToUnLoad != null && sceneToUnLoad.Length != 0)
                {
                    foreach (string scene in sceneToUnLoad)
                    {
                        ScenesManager.Instance.UnloadScene(scene);
                    }
                }
                triggered = true;
            }
        }
    }
}

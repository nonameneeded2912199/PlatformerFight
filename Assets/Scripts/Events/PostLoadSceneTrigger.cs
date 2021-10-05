using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostLoadSceneTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ScenesManager.Instance.IsSceneReadyToBeLoaded = true;
            Invoke("TurnOffSceneManager", 3f);
        }
    }

    private void TurnOffSceneManager()
    {
        ScenesManager.Instance.gameObject.SetActive(false);
    }    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PreloadCoroutine());
    }

    private IEnumerator PreloadCoroutine()
    {
        //yield return new WaitForSeconds(5);
        while (!BGMManager.Instance.isBGMLoaded || !BulletGraphicLoader.Instance.isBulletGraphicsLoaded)
        {
            yield return new WaitForSeconds(3f);
        }

        ScenesManager.Instance.LoadScene("TitleScreen", LoadSceneMode.Single);
    } 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadScript : MonoBehaviour
{
    [SerializeField]
    private GameObject sceneManagerOBJ;

    [SerializeField]
    private GameObject gameManagerOBJ;

    private void Awake()
    {
        GameObject sceneManager = GameObject.FindGameObjectWithTag("ScenesManager");
        if (sceneManager == null)
        {
            Instantiate(sceneManagerOBJ, Vector3.zero, Quaternion.identity);
        }

        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (sceneManager == null)
        {
            Instantiate(gameManagerOBJ, Vector3.zero, Quaternion.identity);
        }
    }
}

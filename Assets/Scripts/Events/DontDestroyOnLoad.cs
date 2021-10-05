using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        print("DONT DESTROY AWAKE");

        GameObject[] listGO = GameObject.FindGameObjectsWithTag(gameObject.tag);
        if (listGO.Length > 1)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}

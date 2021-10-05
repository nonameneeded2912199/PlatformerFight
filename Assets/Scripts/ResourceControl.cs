using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceControl : MonoBehaviour
{
    public Coroutine coUnload;

    // Start is called before the first frame update
    void Start()
    {
        if (coUnload == null)
        {
            coUnload = StartCoroutine(CoUnload());
        }
    }

    private IEnumerator CoUnload()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(2f);
            Resources.UnloadUnusedAssets();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        //get => instance;
        get
        {
            if (instance == null)
            {
                T[] managers = FindObjectsOfType(typeof(T)) as T[];
                if (managers.Length != 0)
                {
                    if (managers.Length == 1)
                    {
                        instance = managers[0];
                        instance.gameObject.name = typeof(T).Name;
                        return instance;
                    }
                    else
                    {
                        Debug.LogError("Class " + typeof(T).Name + " exists multiple times in violation of singleton pattern. Destroying all copies");
                        foreach (T manager in managers)
                        {
                            Destroy(manager.gameObject);
                        }

                    }
                }
                var go = new GameObject(typeof(T).Name, typeof(T));
                instance = go.GetComponent<T>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
        set
        {
            instance = value as T;
        }
    }

    /*protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public static bool IsInitialized
    {
        get => instance != null;
    }*/
}

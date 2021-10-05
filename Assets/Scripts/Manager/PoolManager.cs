using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    Bullet,
    Shadow
}

[Serializable]
public class PoolInfo
{
    public PoolObjectType type;
    public int amount;
    public GameObject prefab;
    public Transform container;

    //[HideInInspector]
    public List<GameObject> pool = new List<GameObject>();
}

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField]
    List<PoolInfo> listOfPool;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetPool();

        for (int i = 0; i < listOfPool.Count; i++)
        {
            Populate(listOfPool[i]);
        }    
    }

    void Populate(PoolInfo info)
    {
        for (int i = 0; i < info.amount; i++)
        {
            GameObject objInstance = Instantiate(info.prefab, info.container.transform);
            objInstance.gameObject.SetActive(false);
            objInstance.transform.position = new Vector3(0, 0, 0);
            if (!info.pool.Contains(objInstance))
            {
                info.pool.Add(objInstance);
            }
        }
    }

    public GameObject GetPoolObject(PoolObjectType type)
    {
        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        GameObject objInstance = null;
        if (pool.Count > 0)
        {
            objInstance = pool[pool.Count - 1];
            pool.Remove(objInstance);
        }
        else
        {
            objInstance = Instantiate(selected.prefab, selected.container);
        }

        return objInstance;
    }

    private PoolInfo GetPoolByType(PoolObjectType type)
    {
        foreach (PoolInfo info in listOfPool)
        {
            if (info.type == type)
            {
                return info;
            }
        }

        return null;
    }

    public void CoolObject(GameObject ob, PoolObjectType type)
    {
        ob.SetActive(false);
        ob.transform.position = new Vector3(0, 0, 0);

        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        if (!pool.Contains(ob))
        {
            pool.Add(ob);
        }
    }

    public void ResetPool()
    {
        for (int i = 0; i < listOfPool.Count; i++)
        {
            foreach (Transform child in listOfPool[i].container)
            {
                Destroy(child.gameObject);
            }
            listOfPool[i].pool.Clear();
        }
        Debug.Log("Reset!");
    }    
}

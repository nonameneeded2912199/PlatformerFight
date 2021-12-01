using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum PoolObjectType
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
}*/

public class PoolManager : Singleton<PoolManager>
{
    public bool logStatus;
    public Transform root;

    private Dictionary<GameObject, ObjectPool<GameObject>> prefabLookup;
    private Dictionary<GameObject, ObjectPool<GameObject>> instanceLookup;

    private bool dirty = false;

    #region Static API

    public static void warmPool(GameObject prefab, int size)
    {
        Instance.WarmPool(prefab, size);
    }

    public static GameObject SpawnObject(GameObject prefab)
    {
        return Instance.spawnObject(prefab);
    }

    public static GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return Instance.spawnObject(prefab, position, rotation);
    }

    public static bool ReleaseObject(GameObject clone)
    {
        return Instance.releaseObject(clone);
    }

    #endregion


    private void Awake()
    {
        prefabLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
        instanceLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
    }

    private void Update()
    {
        if (logStatus && dirty)
        {
            dirty = false;
        }
    }

    public void WarmPool(GameObject prefab, int size)
    {
        if (prefabLookup.ContainsKey(prefab))
        {
            throw new Exception("Pool for prefab " + prefab.name + " has already been created");
        }

        var pool = new ObjectPool<GameObject>(() =>
        {
            return InstantiatePrefab(prefab);
        }, size);

        prefabLookup[prefab] = pool;
        dirty = true;
    }

    public GameObject spawnObject(GameObject prefab)
    {
        return spawnObject(prefab, Vector3.zero, Quaternion.identity);
    }

    public GameObject spawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!prefabLookup.ContainsKey(prefab))
        {
            WarmPool(prefab, 1);
        }

        var pool = prefabLookup[prefab];

        var clone = pool.GetItem();
        clone.transform.SetPositionAndRotation(position, rotation);
        clone.SetActive(true);

        instanceLookup.Add(clone, pool);
        dirty = true;
        return clone;
    }

    public bool releaseObject(GameObject clone)
    {
        clone.SetActive(false);

        if (instanceLookup.ContainsKey(clone))
        {
            instanceLookup[clone].ReleaseItem(clone);
            instanceLookup.Remove(clone);
            dirty = true;
            return true;
        }
        else
        {
            Debug.LogWarning("No pool contains the object:" + clone.name);
            return false;
        }
    }

    private GameObject InstantiatePrefab(GameObject prefab)
    {
        var go = Instantiate(prefab) as GameObject;
        if (root != null)
            go.transform.parent = root;
        return go;
    }

    public void PrintStatus()
    {
        foreach (KeyValuePair<GameObject, ObjectPool<GameObject>> keyVal in prefabLookup)
        {
            Debug.Log(string.Format("Object Pool for Prefab: {0} In Use: {1} Total {2}", keyVal.Key.name, keyVal.Value.CountUsedItems, keyVal.Value.Count));
        }
    }

    private void OnDestroy()
    {
        //prefabLookup.Clear();
        //instanceLookup.Clear();
    }

    void OnGUI()
    {
        GUI.skin.label.normal.textColor = new Color(1f, 1f, 1f, 1f);
        GUI.skin.label.fontSize = 32;
        GUI.Label(new Rect((Screen.width - 120f) / 2f, 10, 500f, 500f), $"Prefab lookup Count: {prefabLookup.Count}");
        GUI.Label(new Rect(40f, 10f, 500f, 500f), $"Instance lookup Count: {instanceLookup.Count}");
    //    //GUI.Label(new Rect(Screen.width - 160f, Screen.height - 50f, 120f, 40f), Difficulty.ToString());
    //    //GUI.Label(new Rect(Screen.width - 320f, 10f, 300f, 40f), "物理[三体运动]");
    }



    //[SerializeField]
    //List<PoolInfo> listOfPool;

    ///*protected override void Awake()
    //{
    //    base.Awake();
    //}*/

    //// Start is called before the first frame update
    //void Start()
    //{
    //    ResetPool();

    //    for (int i = 0; i < listOfPool.Count; i++)
    //    {
    //        Populate(listOfPool[i]);
    //    }    
    //}

    //void Populate(PoolInfo info)
    //{
    //    for (int i = 0; i < info.amount; i++)
    //    {
    //        GameObject objInstance = Instantiate(info.prefab, info.container.transform);
    //        objInstance.gameObject.SetActive(false);
    //        objInstance.transform.position = new Vector3(0, 0, 0);
    //        if (!info.pool.Contains(objInstance))
    //        {
    //            info.pool.Add(objInstance);
    //        }
    //    }
    //}

    //public GameObject GetPoolObject(PoolObjectType type)
    //{
    //    PoolInfo selected = GetPoolByType(type);
    //    List<GameObject> pool = selected.pool;

    //    GameObject objInstance = null;
    //    if (pool.Count > 0)
    //    {
    //        objInstance = pool[pool.Count - 1];
    //        pool.Remove(objInstance);
    //    }
    //    else
    //    {
    //        objInstance = Instantiate(selected.prefab, selected.container);
    //    }

    //    return objInstance;
    //}

    //private PoolInfo GetPoolByType(PoolObjectType type)
    //{
    //    foreach (PoolInfo info in listOfPool)
    //    {
    //        if (info.type == type)
    //        {
    //            return info;
    //        }
    //    }

    //    return null;
    //}

    //public void CoolObject(GameObject ob, PoolObjectType type)
    //{
    //    ob.SetActive(false);
    //    ob.transform.position = new Vector3(0, 0, 0);

    //    PoolInfo selected = GetPoolByType(type);
    //    List<GameObject> pool = selected.pool;

    //    if (!pool.Contains(ob))
    //    {
    //        pool.Add(ob);
    //    }
    //}

    //public void ResetPool()
    //{
    //    for (int i = 0; i < listOfPool.Count; i++)
    //    {
    //        foreach (Transform child in listOfPool[i].container)
    //        {
    //            Destroy(child.gameObject);
    //        }
    //        listOfPool[i].pool.Clear();
    //    }
    //    Debug.Log("Reset!");
    //}    
}

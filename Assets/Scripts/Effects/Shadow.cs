using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public static Shadow Instance;

    private float timer;
    public float speed;
    public Color _color;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetShadows()
    {
        GameObject shadow = PoolManager.Instance.GetPoolObject(PoolObjectType.Shadow);
        shadow.SetActive(true);
        shadow.transform.position = transform.position;
        shadow.transform.rotation = transform.rotation;
        shadow.transform.localScale = transform.localScale;
        shadow.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        shadow.GetComponent<Solid>()._color = _color;
        return shadow;
    }

    public void UpdateTimer()
    {
        timer += speed * Time.deltaTime;
        if (timer > 1)
        {
            GetShadows();
            timer = 0;
        }
    }
}

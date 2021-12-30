using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletCommand : MonoBehaviour
{
    public ulong frame = 0;

    public Action start, update;

    public UnityAction<Collider2D> onTriggerEntered;

    public List<int> ints { get; set; } = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        start?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
            if (!GetComponent<Bullet>().IsDelayed && Time.timeScale != 0)
            {
                update?.Invoke();
                frame++;
            }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (onTriggerEntered != null)
            onTriggerEntered.Invoke(other);
    }
}

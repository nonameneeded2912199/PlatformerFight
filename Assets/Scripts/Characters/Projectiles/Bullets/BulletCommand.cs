using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCommand : MonoBehaviour
{
    public ulong frame = 0;

    public Action start, update;

    public List<int> ints { get; set; } = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        start?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        update?.Invoke();
        frame++;
    }
}

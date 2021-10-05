using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.EventSystem;

public class Test : MonoBehaviour
{
    private void Start()
    {
        GameEventManager.RegisterEvent("Event1", Check1);
        GameEventManager.RegisterEvent("Event2", Check2);

        GameEventManager.SubscribeEvent("Event1", Response1);
        GameEventManager.SubscribeEvent("Event2", Response2);

    }

    private bool Check1(out object[] args)
    {
        args = new object[] { "AAA", 111 };

        return true;
    }

    private void Response1(object[] args)
    {
        if (args.Length == 2)
        {
            Debug.Log("Response 1: " + args[0]);
            Debug.Log("Response 1: " + args[1]);
        }
    }

    private bool Check2(out object[] args)
    {
        args = new object[] { true, 12345, "haaaaa" };

        return true;
    }

    private void Response2(object[] args)
    {
        if (args.Length == 3)
        {
            Debug.Log("Response 2: " + args[0]);
            Debug.Log("Response 2: " + args[1]);
            Debug.Log("Response 2: " + args[2]);
        }
    }
}

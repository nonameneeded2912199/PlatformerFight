using CharacterThings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuff
{
    public BaseCharacter host;
    public float timeLeft;
    public string buffName;

    public Tuple<bool, bool> Tick(float deltaTime)
    {
        timeLeft -= deltaTime;
        bool alive = Mathf.Approximately(timeLeft, 0f) || Mathf.Approximately(Mathf.Sign(timeLeft), -1f);
        return new Tuple<bool, bool>(BuffTick(deltaTime), alive);
    }

    public virtual bool BuffTick(float deltaTime)
    {
        return true;
    }

    public virtual void SetDefault()
    {

    }
}

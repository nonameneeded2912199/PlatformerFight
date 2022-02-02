using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletCommand
{
    private int localFrames = 0;
    private int duration;
    private int timesExecuted = 0;
    private int executeLimit;

    private bool hasExecuteLimit = false;

    private Action<Bullet> command;

    public bool IsEnoughTime => hasExecuteLimit && timesExecuted >= executeLimit;

    public bool IsExecutable => localFrames >= duration;

    public BulletCommand(Action<Bullet> command, int duration, int executeLimit = 1, int startOffset = 0)
    {
        localFrames = 0;
        localFrames += startOffset;

        this.command = command;

        this.duration = duration;

        if (executeLimit >= 1)
        {
            hasExecuteLimit = true;
            this.executeLimit = executeLimit;
        }
        else
        {
            hasExecuteLimit = false;
        }
    }

    public void Update()
    {
        localFrames++;
    }

    public void Execute(Bullet bullet)
    {
        timesExecuted++;

        command.Invoke(bullet);
        localFrames = 0;
    }
}

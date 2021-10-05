using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialog
{
    IDialog SetText(string text);

    IDialog SetTitle(string text);

    IDialog AddButton(string text, Action action);

    IDialog AddCancelButton(string text);

    IDialog Show();
}

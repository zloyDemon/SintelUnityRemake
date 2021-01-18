using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    public bool IsOpened { get; private set; }

    public Action<BaseWindow> OnWindowOpened = w => { };
    public Action<BaseWindow> OnWindowClosed = w => { };

    public void Open()
    {
        IsOpened = true;
        gameObject.SetActive(true);
        OnWindowOpened(this);
    }

    public void Close()
    {
        IsOpened = false;
        gameObject.SetActive(false);
        OnWindowClosed(this);
    }
}

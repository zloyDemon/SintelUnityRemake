using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGO : MonoBehaviour
{
    public Action<Collider> OnTriggerEntered = c => { };
    public Action<Collider> OnTriggerExited = c => { };

    private Action<Collider> onTriggerEnteredInternal = c => { };
    private Action<Collider> onTriggerExitedInternal = c => { };

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        OnTriggerEntered(other);
        onTriggerEnteredInternal(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        OnTriggerExited(other);
        onTriggerExitedInternal(other);
    }

    public void OnEnterTriggerListener(Action<Collider> onEnterTrigger)
    {
        onTriggerEnteredInternal = onEnterTrigger;
    }

    public void OnExitTriggerListener(Action<Collider> onExitTrigger)
    {
        onTriggerExitedInternal = onExitTrigger;
    }

    public void Dispose()
    {
        OnTriggerEntered = c => { };
        OnTriggerExited = c => { };
        onTriggerExitedInternal = null;
        onTriggerEnteredInternal = null;
    }
}

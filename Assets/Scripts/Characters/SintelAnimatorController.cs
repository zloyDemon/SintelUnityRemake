using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SintelAnimatorController : MonoBehaviour
{
    public enum AnimationEventId
    {
        None,
        Attack,
        Footsteps,
    }

    private Animator _animator;
    private bool inState;

    public event Action<int> AnimationEvent = v => { };
    public Animator MainAnimtor => _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetValue(int paramId, int value)
    {
        _animator.SetInteger(paramId, value);
    }

    public void SetValue(int paramId, float value)
    {
        _animator.SetFloat(paramId, value);
    }

    public void SetValue(int paramId, bool value)
    {
        _animator.SetBool(paramId, value);
    }

    public void SetTrigger(int paramId, bool isSet)
    {
        if (isSet)
            _animator.SetTrigger(paramId);
        else
            _animator.ResetTrigger(paramId);
    }

    public void AnimationEventHandler(int value)
    {
        AnimationEvent(value);
    }

    public void GetNextAnimationState()
    {
        
    }
}

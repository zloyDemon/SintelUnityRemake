using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class SintelAnimatorState : StateMachineBehaviour
{
    [SerializeField] string stateName;

    private bool isInited;
    private void Awake()
    {
        if (isInited)
            return;

        Debug.Log($"Awake state {stateName}");
        int hashId = Animator.StringToHash(stateName);
        StateManager.Instance.AddState(hashId, this);
        isInited = true;
    }

    public event Action<Animator, AnimatorStateInfo, int, string> OnStateEnterEvent = (a, asi, li, sn) => { };
    public event Action<Animator, AnimatorStateInfo, int, string> OnStateExitEvent = (a, asi, li, sn) => { };
    public event Action<Animator, AnimatorStateInfo, int, string> OnStateUpdateEvent = (a, asi, li, sn) => { };

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnStateEnterEvent(animator, stateInfo, layerIndex, stateName);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnStateUpdateEvent(animator, stateInfo, layerIndex, stateName);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnStateExitEvent(animator, stateInfo, layerIndex, stateName);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

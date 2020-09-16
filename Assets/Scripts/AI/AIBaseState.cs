using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AIBaseState : MonoBehaviour
{
    public AIBaseBehaviour AIBaseBehaviour { get; set; }
    public Dictionary<AIBaseState, Func<bool>> Transitions { get; } = new Dictionary<AIBaseState, Func<bool>>();

    public virtual void AddTransition(AIBaseState to, Func<bool> condition)
    {
        if (!Transitions.ContainsKey(to))
            Transitions.Add(to, condition);
    }

    public virtual void CheckTransitions()
    {
        foreach (var kvp in Transitions)
        {
            if (kvp.Value())
                AIBaseBehaviour.ChangeState(kvp.Key);
        }
    }

    public virtual void Tick()
    {
        CheckTransitions();
        UpdateState();
    }

    abstract public void EnterState();
    abstract public void UpdateState();
    abstract public void ExitState();
}

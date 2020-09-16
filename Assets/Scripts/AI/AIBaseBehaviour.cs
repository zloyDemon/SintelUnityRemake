using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

abstract public class AIBaseBehaviour : MonoBehaviour
{
    [Header("Base class")]
    [SerializeField] Transform statesHolder;

    public NavMeshAgent Agent { get; private set; }
    public SintelAgent SintelAgent { get; private set; }
    public Dictionary<Type, AIBaseState> States { get; private set; } = new Dictionary<Type, AIBaseState>();
    public MoveComponent MoveComponent { get; private set; }
    public AIBaseState CurrentState { get; private set; }
    public SintelPlayer SintelPlayer { get; private set; }
    public Transform Target { get; set; }

    public virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        SintelAgent = GetComponent<SintelAgent>();
        MoveComponent = GetComponent<MoveComponent>();
        GetStates();
        SintelPlayer = SintelGameManager.Instance.SintelPlayer;
    }

    private void GetStates()
    {
        var states = statesHolder.GetComponents<AIBaseState>();
        for (int i = 0; i < states.Length; i++)
        {
            AddState(states[i]);
            states[i].AIBaseBehaviour = this;
        }
    }

    public void ChangeState(AIBaseState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.ExitState();
            CurrentState = null;
        }

        CurrentState = newState;
        CurrentState?.EnterState();
    }

    public void AddState(AIBaseState state)
    {
        if (!States.ContainsKey(state.GetType()))
            States.Add(state.GetType(), state);
    }

    public T GetState<T>() where T : AIBaseState
    {
        return States[typeof(T)] as T;
    }

    public virtual void Death()
    {
        CurrentState = null;
    }
}

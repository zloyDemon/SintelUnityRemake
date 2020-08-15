using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    public static StateManager Instance { get; private set; } = new StateManager();

    private Dictionary<int, SintelAnimatorState> states = new Dictionary<int, SintelAnimatorState>();

    public void Init() { 
    }

    public void AddState(int id, SintelAnimatorState sintelAnimatorState)
    {
        if(!states.ContainsKey(id))
            states.Add(id, sintelAnimatorState);
    }

    public SintelAnimatorState GetStateById(int id)
    {
        return states.ContainsKey(id) ? states[id] : null;
    }
}

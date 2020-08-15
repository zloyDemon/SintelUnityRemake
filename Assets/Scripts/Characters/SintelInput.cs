using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SintelInput : MonoBehaviour
{
    public Vector3 InputDirection { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool SprintPressed { get; private set; }
    public bool AttackPressed { get; private set; }

    public enum SintelInputType
    {
        Horizontal,
        Vertical,
        Sprint,
        Attack,
        Jump,
    }

    private Dictionary<SintelInputType, object> inputs = new Dictionary<SintelInputType, object>
    {
        { SintelInputType.Horizontal, default(float) },
        { SintelInputType.Vertical, default(float) },
        { SintelInputType.Attack, default(bool) },
        { SintelInputType.Sprint, default(bool) },
        { SintelInputType.Jump, default(bool) },
    };


    private void Awake()
    {

    }

    private void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        inputs[SintelInputType.Horizontal] = x;
        inputs[SintelInputType.Vertical] = z;

        if (Input.GetKeyDown(KeyCode.Space))
            inputs[SintelInputType.Jump] = true;

        inputs[SintelInputType.Sprint] = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            inputs[SintelInputType.Attack] = true;
    }

    public void ResetInputs()
    {
        JumpPressed = false;
        SprintPressed = false;
        AttackPressed = false;
        inputs[SintelInputType.Attack] = false;
        inputs[SintelInputType.Jump] = false;
        inputs[SintelInputType.Sprint] = false;
    }

    public void AddInput<T>(SintelInputType type, T value)
    {
        inputs.Add(type, value);
    }

    public T GetInput<T>(SintelInputType name)
    {
        var input = inputs[name];
        if (!(input is T))
            throw new Exception($"Current input {name} has wrong type.");

        return (T)input;
    }
}

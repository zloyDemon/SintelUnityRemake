using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveOnPlaceState : AIBaseState
{
    private Vector3 currentDirection = Vector3.zero;
    private float time;
    private bool isMoving;
    private Action<Vector3> moving;


    public override void EnterState()
    {

    }

    public override void ExitState()
    {
        moving?.Invoke(Vector3.zero);
    }

    public override void UpdateState()
    {
        if(AIBaseBehaviour.CurrentState == this)
        {
            Debug.DrawRay(transform.position, currentDirection, Color.cyan);
            CheckMoveOrWaitForChange();
        }
    }

    public void SetMovingCallback(Action<Vector3> movingCallback)
    {
        moving = movingCallback;
    }

    private void CheckMoveOrWaitForChange()
    {
        if (time <= 0)
        {
            if (isMoving)
            {
                time = Random.Range(2, 4);
                currentDirection = Vector3.zero;
            }
            else
            {

                currentDirection = GenerateNewDirection();
                time = Random.Range(2, 4);
            }

            isMoving = !isMoving;
        }

        moving?.Invoke(currentDirection);
        time -= Time.deltaTime;
    }

    private Vector3 GenerateNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        Quaternion quat = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 newZ = quat * Vector3.forward;
        newZ.y = 0;
        return newZ.normalized;
    }
}

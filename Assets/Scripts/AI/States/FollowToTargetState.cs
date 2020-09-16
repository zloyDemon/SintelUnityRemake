using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowToTargetState : AIBaseState
{
    private Transform targetToMove;
    private Action<Vector3> MoveToDirection = d => { };

    public void SetTargetToMove(Transform target)
    {
        targetToMove = target;
    }

    public FollowToTargetState SetMoveCallback(Action<Vector3> moveCallback)
    {
        MoveToDirection = moveCallback;
        return this;
    }

    public override void EnterState()
    {
        /*AIBaseBehaviour.Agent.isStopped = false;
        AIBaseBehaviour.Agent.speed = SintelGameParameters.CharactersValue.BugSpeed;*/
    }

    public override void ExitState()
    {
        AIBaseBehaviour.Agent.ResetPath();
        AIBaseBehaviour.Agent.isStopped = true;
    }

    public override void UpdateState()
    {
        if (targetToMove != null && targetToMove.hasChanged && AIBaseBehaviour.CurrentState == this)
        {
            AIBaseBehaviour.SintelAgent.RequestPath(targetToMove);
            targetToMove.hasChanged = false;
        }
    }
}

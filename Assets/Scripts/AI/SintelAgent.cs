using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SintelAgent : MonoBehaviour
{
    private const float DistanceForChangePoint = 0.3f;

    NavMeshAgent agent;
    MoveComponent moveComponent;
    private bool isMovingOnPath;
    Transform currentTarget;
    private int index = -1;
    private Vector3 currenCornerPosition;
    private Vector3 currentCornerDirection;

    public List<Vector3> CurrentPath { get; private set; } = new List<Vector3>();
    private List<Vector3> pathCorners = new List<Vector3>();

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        moveComponent = GetComponent<MoveComponent>();
    }

    private void FixedUpdate()
    {
       /* if (isMovingOnPath)
            Move();*/
    }

    private List<Vector3> CalculatePathToTarget(Transform target)
    {
        List<Vector3> result = new List<Vector3>();
        NavMeshPath path = new NavMeshPath();
        bool success = NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);

        if (success)
        {
            result = path.corners.ToList();
        }

        return result;
    }

    public void RequestPath(Transform target)
    {
        agent.ResetPath();
        agent.path.ClearCorners();
        agent.SetDestination(target.position);
        return;
    }

    //TODO
    private void FindCorenrs(Transform target)
    {
        Stop();
        var corners = CalculatePathToTarget(target);
        if (corners.Count > 0)
        {
            pathCorners = corners.ToList();
            UpdateCurrentDirection();
            isMovingOnPath = true;
        }
    }

    private void Move()
    {
        moveComponent.MoveWithRotation(GetDirectionToTarget(currenCornerPosition).normalized);
        CheckDistanceToPoint();
    }

    private void CheckDistanceToPoint()
    {
        if (Vector3.SqrMagnitude(GetDirectionToTarget(currenCornerPosition)) < DistanceForChangePoint)
            UpdateCurrentDirection();        
    }

    private void UpdateCurrentDirection()
    {
        index++;

        if (index >= pathCorners.Count)
        {
            Stop();
            return;
        }

        currenCornerPosition = pathCorners[index];
        currentCornerDirection = currenCornerPosition - transform.position;
    }

    private Vector3 GetDirectionToTarget(Vector3 target)
    {
        return target - transform.position;
    }

    public void Stop()
    {
        isMovingOnPath = false;
        index = -1;
        pathCorners.Clear();
    }

    private void OnDrawGizmos()
    {
        if (pathCorners.Count > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, GetDirectionToTarget(currenCornerPosition).normalized);
            Gizmos.color = Color.cyan;
            for (int i = 0; i < pathCorners.Count; i++)
            {
                if(i > 0)
                    Gizmos.DrawLine(pathCorners[i], pathCorners[i - 1]);
                Gizmos.DrawSphere(pathCorners[i], 0.25f);
            }
        }
    }
}

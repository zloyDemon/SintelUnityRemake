using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrolState : AIBaseState
{
    private List<Transform> patrolPoints;
    private Transform currentPatrolPoint;
    private Transform lastVisitedPoint;
    private Coroutine waitOnPoint;
    private float agentSpeed;
    
    public PatrolState SetPatrolPoints(List<Transform> patrolPoints)
    {
        this.patrolPoints = new List<Transform>(patrolPoints);
        return this;
    }

    public PatrolState SetAgentSpeed(float speed)
    {
        return this;
    }

    public override void EnterState()
    {
        GetNewPatrolPoint();
    }

    public override void ExitState()
    {
        currentPatrolPoint = null;
        SintelUtils.KillAndNullCoroutine(this, ref waitOnPoint);
    }

    public override void UpdateState()
    {
        if (currentPatrolPoint != null && waitOnPoint == null && (transform.position - currentPatrolPoint.position).sqrMagnitude < 0.5f)
        {
            lastVisitedPoint = currentPatrolPoint;
            WaitOnPatrolPoint();
        }
    }

    private void GetNewPatrolPoint()
    {
        if (lastVisitedPoint != null)
            patrolPoints.Remove(lastVisitedPoint);

        var index = Random.Range(0, patrolPoints.Count);
        currentPatrolPoint = patrolPoints[index];
        if (lastVisitedPoint != null)
            patrolPoints.Add(lastVisitedPoint);

        AIBaseBehaviour.SintelAgent.RequestPath(currentPatrolPoint);
    }

    private void WaitOnPatrolPoint()
    {
        SintelUtils.KillAndNullCoroutine(this, ref waitOnPoint);
        waitOnPoint = StartCoroutine(CorWaitOnPoint());
    }

    IEnumerator CorWaitOnPoint()
    {
        //int time = Random.Range(2, 5);
        int time = 0;
        yield return new WaitForSeconds(time);
        GetNewPatrolPoint();
        waitOnPoint = null;
    }
}

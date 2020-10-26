using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiDistanceToPlayer : MonoBehaviour
{
    private Transform entity;
    private Transform player;
    private float distanceToCheck;
    private bool isPlayerNear = false;

    public event Action<bool> PlayerNear = b => { };

    private void Awake()
    {
        player = SintelGameManager.Instance.SintelPlayer.transform;
    }

    public void Init(Transform entity)
    {
        this.entity = entity;
    }

    public void SetDistanceToCheck(float distance)
    {
        distanceToCheck = distance;
    }

    private void Update()
    {
        if(IsPlayerNearDistance() && !isPlayerNear)
        {
            isPlayerNear = true;
            PlayerNear(isPlayerNear);
        }
        else if(!IsPlayerNearDistance() && isPlayerNear)
        {
            isPlayerNear = false;
            PlayerNear(isPlayerNear);
        }
    }

    private bool IsPlayerNearDistance()
    {
        return (player.position - entity.position).sqrMagnitude <= distanceToCheck;
    }
}

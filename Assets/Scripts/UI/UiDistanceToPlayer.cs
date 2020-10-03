using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiDistanceToPlayer : MonoBehaviour
{
    private Transform entity;
    private Transform player;
    private float distanceToCheck;
    private bool isPlayerNear = false;

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
            RequestForView();
        }
        else if(!IsPlayerNearDistance() && isPlayerNear)
        {
            isPlayerNear = false;
            DisableView();
        }
    }

    private bool IsPlayerNearDistance()
    {
        return (player.position - entity.position).sqrMagnitude <= distanceToCheck;
    }

    public void RequestForView()
    {
         SintelGameManager.Instance.GameUI.GameObjecUIController.RequestView<NpcHpBar>(transform, b => b.SetCharacterData(entity.GetComponent<CharacterData>()));
    }

    public void DisableView()
    {
        SintelGameManager.Instance.GameUI.GameObjecUIController.DisableView(transform);
    }
}

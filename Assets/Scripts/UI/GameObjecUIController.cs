using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjecUIController : MonoBehaviour
{
    [SerializeField] Canvas goUICanvas;
    [SerializeField] NpcHpBar npcHpBar;

    public Canvas GoUiCanvas => goUICanvas;

    private Dictionary<Transform, NpcHpBar> uiForGoList = new Dictionary<Transform, NpcHpBar>();

    private void Awake()
    {
        PoolManager.Instance.AddToPool<NpcHpBar>(npcHpBar, 10);
    }

    public T RequestView<T>(Transform target, Action<T> initialization = null, Vector3 offset = default) where T : UiForGO
    {
        var newView = PoolManager.Instance.GetFromPool<NpcHpBar>();
        newView.SetTargetTransform(target, offset);
        newView.transform.SetParent(goUICanvas.transform);
        uiForGoList.Add(target, newView);
        initialization(newView as T);
        return newView as T;
    }

    public void DisableView(Transform target)
    {
        if (!uiForGoList.ContainsKey(target))
            return;

        var currentBar = uiForGoList[target];
        currentBar.SetTargetTransform(null);
        currentBar.DeleteCharacterData();
        uiForGoList.Remove(target);
        PoolManager.Instance.ReturnToPool(currentBar);
    }
}

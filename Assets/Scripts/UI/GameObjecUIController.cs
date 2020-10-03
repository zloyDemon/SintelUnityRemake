using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjecUIController : MonoBehaviour
{
    [SerializeField] Canvas goUICanvas;
    [SerializeField] NpcHpBar npcHpBarPrefab;

    public Canvas GoUiCanvas => goUICanvas;

    private Dictionary<Transform, NpcHpBar> uiForGoList = new Dictionary<Transform, NpcHpBar>();

    private List<UiForGO> npcHpBarPool = new List<UiForGO>();


    private void Awake()
    {
        InitNpcBars();
    }

    private void InitNpcBars()
    {
        int count = 3;
        for(int i = 0; i < count; i++)
        {
            var go = Instantiate(npcHpBarPrefab, GoUiCanvas.transform);
            npcHpBarPool.Add(go);
            go.gameObject.SetActive(false);
        }
    }

    public void RequestToViewObject(Transform target, CharacterData data) 
    {
        var hpBar = Instantiate(npcHpBarPrefab);
        hpBar.SetTargetTransform(target);
        hpBar.transform.SetParent(goUICanvas.transform);
        hpBar.SetCharacterData(data);
        uiForGoList.Add(target, hpBar);
    }

    public T RequestView<T>(Transform target, Action<T> initialization = null, Vector3 offset = default) where T : UiForGO
    {
        var newView = GetFromPool() as NpcHpBar;
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
        PutOnPool(currentBar);
    }

    private UiForGO GetFromPool()
    {
        var o = npcHpBarPool.Find(e => e.gameObject.activeSelf == false);
        if(o == null)
        {
            var go = Instantiate(npcHpBarPrefab);
            npcHpBarPool.Add(go);
            return go;
        }

        return o;
    }

    private void PutOnPool(UiForGO o)
    {
        o.gameObject.SetActive(false);
    }
}

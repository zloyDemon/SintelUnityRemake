using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjecUIController : MonoBehaviour
{
    [SerializeField] Canvas goUICanvas;
    [SerializeField] NpcHpBar npcHpBar;
    [SerializeField] ObjectivePoint objectivePoint;

    public Canvas GoUiCanvas => goUICanvas;

    private Dictionary<Transform, List<UiForGO>> uiForGoList = new Dictionary<Transform, List<UiForGO>>();

    private void Awake()
    {
        PoolManager.Instance.AddToPool<NpcHpBar>(npcHpBar, 10);
        PoolManager.Instance.AddToPool<ObjectivePoint>(objectivePoint, 2);
    }

    public T RequestView<T>(Transform target, Action<T> initialization = null, Vector3 offset = default) where T : UiForGO
    {
        var newView = PoolManager.Instance.GetFromPool<T>();
        newView.transform.SetParent(goUICanvas.transform);
        var followGo = newView.GetComponent<FollowGOView>();
        followGo.SetFollowTarget(target.gameObject);
        followGo.SetOffset(offset.x, offset.y);
        newView.gameObject.SetActive(true);
        CheckTargetInList(target);
        uiForGoList[target].Add(newView);
        initialization?.Invoke(newView as T);
        return newView as T;
    }

    public T RequestView<T>() where T : UiForGO
    {
        var requestedView = PoolManager.Instance.GetFromPool<T>();
        requestedView.transform.SetParent(goUICanvas.transform);
        return requestedView;
    }

    public void DisableView<T>(Transform target)
    {
        if (!uiForGoList.ContainsKey(target))
            return;

        var view = uiForGoList[target].Find(e => e.GetType() == typeof(T));

        if (view == null)
        {
            Debug.LogError("Current view doesn't exists in list");
            return;
        }

        var followGo = view.GetComponent<FollowGOView>();
        followGo.SetFollowTarget(null);
        uiForGoList[target].Remove(view);
        PoolManager.Instance.ReturnToPool(view);
        CheckForClearTargetList(target);
    }

    public T GetCurrentView<T>() where T : UiForGO
    {
        foreach (var kvp in uiForGoList)
        {
            var view = kvp.Value.Find(e => e.GetType() == typeof(T));
            if (view != null)
                return view as T;
        }

        return null;
    }

    public void SetTargetForObjectivePoint(GameObject target)
    {
        RequestView<ObjectivePoint>(target.transform, op => op.Init(target.transform), new Vector3(0, 35, 0));
    }

    public void DisableTargetForObjectivePoint(GameObject target)
    {
        DisableView<ObjectivePoint>(target.transform);
    }

    private void CheckTargetInList(Transform target)
    {
        if (!uiForGoList.ContainsKey(target))
            uiForGoList.Add(target, new List<UiForGO>());
    }

    private void CheckForClearTargetList(Transform target)
    {
        if (!uiForGoList.ContainsKey(target) && uiForGoList[target].Count == 0)
            uiForGoList.Remove(target);
    }

    //TODO need refactor or delete
    public void CLearOldObjective()
    {
        foreach(var kvp in uiForGoList)
        {
            var list = kvp.Value.Find(e => e.GetType() == typeof(ObjectivePoint));
            if(list != null)
                DisableView<ObjectivePoint>(kvp.Key);
        }
    }
}

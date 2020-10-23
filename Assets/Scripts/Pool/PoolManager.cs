using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("Original pefabs")]
    [SerializeField] NpcHpBar npcBarPrefab;

    private Dictionary<Type, Pool> pools = new Dictionary<Type, Pool>();

    public static PoolManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == this)
            Destroy(gameObject);
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);

        InitPool();
    }

    public void AddToPool<T>(T prefab) where T : MonoBehaviour
    {
        AddToDictionary(prefab);
    }

    public T GetFromPool<T>() where T : MonoBehaviour
    {
        Type type = typeof(T);
        if (!pools.ContainsKey(type))
        {
            Debug.LogError($"Pool doesn't have type {type.Name}");
            return null;
        }

        var result = pools[type].RequestObjectFromPool();
        return result.GetComponent<T>();
    }

    public void ReturnToPool<T>(T returnObject) where T : MonoBehaviour
    {
        Type type = returnObject.GetType();
        if (!pools.ContainsKey(type))
        {
            Debug.LogError($"Pool doesn't have type {type.Name}");
            return;
        }

        pools[type].ReturnToPool(returnObject.gameObject);
    }

    private void InitPool()
    {
        AddToDictionary(npcBarPrefab);    
    }

    private void AddToDictionary<T>(T prefab) where T : MonoBehaviour
    {
        Type type = prefab.GetType();

        if (pools.ContainsKey(type))
        {
            Debug.LogError($"The type {type.Name} already exist in pool");
            return;
        }

        var go = new GameObject(type.Name);
        go.transform.SetParent(transform);
        var newPool = new Pool(prefab.gameObject);
        newPool.SetParent(go.transform);
        pools.Add(prefab.GetType(), newPool); 
    }
}

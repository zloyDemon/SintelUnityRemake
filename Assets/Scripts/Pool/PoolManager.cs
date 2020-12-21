using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Dictionary<Type, Pool> pools = new Dictionary<Type, Pool>();

    public static PoolManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == this)
            Destroy(gameObject);
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddToPool<T>(T prefab, int amount, string containerName = "") where T : MonoBehaviour
    {
        AddToDictionary(prefab, amount, containerName);
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

    private void AddToDictionary<T>(T prefab, int amount, string name = "") where T : MonoBehaviour
    {
        Type type = prefab.GetType();

        if (pools.ContainsKey(type))
        {
            Debug.LogError($"The type {type.Name} already exist in pool");
            return;
        }

        var contanerName = string.IsNullOrEmpty(name) ? type.Name : name;
        var go = new GameObject(contanerName);
        go.transform.SetParent(transform);
        var newPool = new Pool(prefab.gameObject, amount);
        newPool.SetParent(go.transform);
        pools.Add(prefab.GetType(), newPool); 
    }
}

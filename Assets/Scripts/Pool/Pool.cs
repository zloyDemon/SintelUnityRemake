using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private int startAmount;
    private Transform objectsParent;
    private GameObject orirginalPrefab;
    List<GameObject> poolableObjects = new List<GameObject>();

    public Pool(GameObject originalPrefab, int amount = 10)
    {
        this.orirginalPrefab = originalPrefab;
        startAmount = amount;
        InitObjects();
    }

    public void SetParent(Transform parent)
    {
        objectsParent = parent;
    }

    private void InitObjects()
    {
        PoolManager.Instance.StartCoroutine(CorSpawnObject());
    }

    private GameObject CreatePoolableObject()
    {
        var go = Object.Instantiate(orirginalPrefab);
        if (objectsParent != null)
            go.transform.SetParent(objectsParent);
        poolableObjects.Add(go);
        go.gameObject.SetActive(false);
        return go;
    }

    public GameObject RequestObjectFromPool()
    {
        var resultGo = poolableObjects.Find(e => !e.gameObject.activeSelf);
        if (resultGo == null)
            resultGo = CreatePoolableObject();
        return resultGo;
    }

    public void ReturnToPool(GameObject obj)
    {
        if (!poolableObjects.Contains(obj))
        {
            Debug.LogError($"Object {obj} doesn't exist in pool's collection");
            return;
        }

        obj.gameObject.SetActive(false);
        obj.transform.SetParent(objectsParent);
    }

    IEnumerator CorSpawnObject()
    {
        for (int i = 0; i < startAmount; i++)
        {
            CreatePoolableObject();
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
    }
}

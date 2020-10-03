using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] BugController bugControllerPrefab;

    public BugController SpawnBug(Vector3 position, Quaternion rotation)
    {
        return SpawnObject<BugController>(bugControllerPrefab.gameObject, position, rotation);
    }

    private T SpawnObject<T>(GameObject originalPrefab, Vector3 position, Quaternion rotation) where T : MonoBehaviour
    {
        var spawnedObject = Instantiate(originalPrefab, position, rotation);
        return spawnedObject as T;
    }
}

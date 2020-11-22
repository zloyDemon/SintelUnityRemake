using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] BugController bugControllerPrefab;

    public BugController SpawnBug(Vector3 position, Quaternion rotation)
    {
        var bug = SpawnObject<BugController>(bugControllerPrefab.gameObject, position, rotation);
        return bug;
    }

    private T SpawnObject<T>(GameObject originalPrefab, Vector3 position, Quaternion rotation) where T : MonoBehaviour
    {
        var spawnedObject = Instantiate(originalPrefab, position, rotation);
        return spawnedObject.GetComponent<T>();
    }
}

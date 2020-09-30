using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] BugController bugControllerPrefab;

    public GameObject SpawnBug(Vector3 position, Quaternion rotation)
    {
        return SpawnObject(bugControllerPrefab.gameObject, position, rotation);
    }

    private GameObject SpawnObject(GameObject originalPrefab, Vector3 position, Quaternion rotation)
    {
        var spawnedObject = Instantiate(originalPrefab, position, rotation);
        return spawnedObject;
    }
}

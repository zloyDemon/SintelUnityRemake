using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] Transform sintelSpawnPoint;
    [SerializeField] Transform bugSpawnPosition;

    public Transform SintelSpawnPoint => sintelSpawnPoint;
    public Transform BugSpawnPosition => bugSpawnPosition;

    public List<Transform> GetPatrolPoints()
    {
        var list = new List<Transform>();
        var pointsObject = transform.Find("PatrolPoints");
        if(pointsObject != null)
        {
            for(int i = 0; i < pointsObject.childCount; i++)
            {
                list.Add(pointsObject.GetChild(i));
            }
        }

        return list;
    }
}

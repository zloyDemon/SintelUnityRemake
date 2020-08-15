using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] Transform sintelSpawnPoint;

    public Transform SintelSpawnPoint => sintelSpawnPoint;
}

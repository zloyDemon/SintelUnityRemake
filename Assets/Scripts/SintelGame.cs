using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SintelGame : MonoBehaviour
{
    public static SintelGame Instance { get; private set; }


    private void Awake()
    {
        if (Instance == this)
            Destroy(gameObject);
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init()
    {
        Debug.Log("SintelGame Init");
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 50;
#endif
    }
}

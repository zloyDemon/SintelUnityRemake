using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SintelGameManager : MonoBehaviour
{
    public static SintelGameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == this)
            Destroy(gameObject);
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);

        InitSintelGame();
    }

    private void InitSintelGame()
    {
        Debug.Log("Sintel game initialization");
    }
}

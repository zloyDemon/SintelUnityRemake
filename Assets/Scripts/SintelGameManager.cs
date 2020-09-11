﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SintelGameManager : MonoBehaviour
{
    [SerializeField] SintelPlayer sintelPlayer;
    [SerializeField] CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] GameUI gameUI;

    private LevelData levelData;
    private Canvas canvas;

    public static SintelGameManager Instance { get; private set; }

    public Camera MainCamera => Camera.main;

    public SintelPlayer SintelPlayer { get; private set; }
    public GameUI GameUI { get; private set; }

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
        Debug.Log("Start init");
        FindLevelData();
        StartCoroutine(InitCharacter());
        GameUI = Instantiate(gameUI);
    }

    private void FindLevelData()
    {
        levelData = FindObjectOfType<LevelData>();
    }

    IEnumerator InitCharacter()
    {
        yield return new WaitForEndOfFrame();
        SpawnSintelPlayer();
        yield return new WaitForEndOfFrame();
        InitCamera();
        yield return new WaitForEndOfFrame();
    }

    private void SpawnSintelPlayer()
    {
        Vector3 spawnPostion = levelData.SintelSpawnPoint.position;
        var sintel = Instantiate(sintelPlayer, spawnPostion, Quaternion.identity);
        SintelPlayer = sintel;
    }

    private void InitCamera()
    {
        Vector3 spawnPostion = new Vector3(0, 0, 0);
        var camera = Instantiate(cinemachineFreeLook, spawnPostion, Quaternion.identity);
        var lookAtGO = SintelPlayer.transform.Find("CameraTarget");
        camera.Follow = SintelPlayer.transform;
        camera.LookAt = lookAtGO;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopViewManager : MonoBehaviour
{
    [SerializeField] DeveloperView developerView;

    public static DevelopViewManager Instance { get; private set; }

    public bool ShowDebugView { get; set; } = false;

    private void Awake()
    {
        if (Instance == this)
            Destroy(gameObject);
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);
        developerView.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isActive = developerView.gameObject.activeSelf;
            developerView.gameObject.SetActive(!isActive);
        }
    }

    public void SetValue(string key, object value)
    {
        developerView.SetParameterValue(key, value.ToString());
    }

    public void AddFunction(string functionName, Action function)
    {
        developerView.AddNewFunction(functionName, function);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
public class DeveloperView : MonoBehaviour
{
    private const float ActiveAlphaColor = 60;
    private const float NoActiveAlphaColor = 20;

    [Header("Source prefabs")]
    [SerializeField] KeyValueItem keyValueItemPrefab;
    [SerializeField] FunctionCallItem functionCallItemPrefab;


    [Header("Views")]
    [SerializeField] Button paramtersTabBtn;
    [SerializeField] Button functionsTabBtn;
    [SerializeField] Image parametersTab;
    [SerializeField] Image functionsTab;
    [SerializeField] Transform contentParameters;
    [SerializeField] Transform contentFunctions;

    private Image parametersBtnImg;
    private Image functionsBtnImg;

    private Dictionary<string, KeyValueItem> parameters = new Dictionary<string, KeyValueItem>();
    private Dictionary<string, FunctionCallItem> functions = new Dictionary<string, FunctionCallItem>();

    private GameObject currentTab;

    private void Awake()
    {
        paramtersTabBtn.onClick.AddListener(OnParametersTabClick);
        functionsTabBtn.onClick.AddListener(OnFunctionsTabClick);

        parametersBtnImg = paramtersTabBtn.GetComponent<Image>();
        functionsBtnImg = functionsTabBtn.GetComponent<Image>();

        functionsTab.gameObject.SetActive(false);
        parametersTab.gameObject.SetActive(false);

        currentTab = parametersTab.gameObject;

        OnParametersTabClick();
    }

    private void OnParametersTabClick()
    {
        ActivateTab(parametersTab.gameObject);
        ClearButtons();
        UpdateTabButtonColor(parametersBtnImg, ActiveAlphaColor);
    }

    private void OnFunctionsTabClick()
    {
        ActivateTab(functionsTab.gameObject);
        ClearButtons();
        UpdateTabButtonColor(functionsBtnImg, ActiveAlphaColor);
    }

    private void ClearButtons()
    {
        UpdateTabButtonColor(parametersBtnImg, NoActiveAlphaColor);
        UpdateTabButtonColor(functionsBtnImg, NoActiveAlphaColor);
    }

    private void UpdateTabButtonColor(Image image, float alpha)
    {
        var color = image.color;
        var newColor = new Color32((byte)color.r, (byte)color.g, (byte)color.b, (byte)alpha);
        image.color = newColor;
    }

    private void ActivateTab(GameObject tab)
    {
        if (currentTab != null)
            currentTab.SetActive(false);

        currentTab = tab;
        currentTab.SetActive(true);
    }

    public void SetParameterValue(string key, string value)
    {
        if (!parameters.ContainsKey(key))
            parameters.Add(key, CreateNewParameter(key, string.Empty));
        parameters[key].SetValueText(value);
    }

    public void AddNewFunction(string name, Action action)
    {
        if (!functions.ContainsKey(name))
            functions.Add(name, CreateNewFunction(name, action));
    }

    public KeyValueItem CreateNewParameter(string key, string value)
    {
        var newItem = Instantiate(keyValueItemPrefab, contentParameters);
        newItem.Init(key);
        newItem.SetValueText(value);
        return newItem;
    }

    public FunctionCallItem CreateNewFunction(string functionName, Action click)
    {
        var newItem = Instantiate(functionCallItemPrefab, contentFunctions);
        newItem.TextButton = functionName;
        newItem.OnButtonClicked = click;
        return newItem;
    }

    public void DeleteParams(string key)
    {
        var item = parameters[key];
        Destroy(item.gameObject);
        parameters.Remove(key);
    }

    public void DeleteFunction(string key)
    {
        var item = functions[key];
        item.OnButtonClicked = null;
        Destroy(item.gameObject);
        functions.Remove(key);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionCallItem : MonoBehaviour
{
    [SerializeField] Text buttonText;
    [SerializeField] Button button;

    public string TextButton { set { buttonText.text = value; } }
    public Action OnButtonClicked;

    private void Awake()
    {
        button.onClick.AddListener(() => OnButtonClicked?.Invoke());
    }

    private void Clicked()
    {

    }
}

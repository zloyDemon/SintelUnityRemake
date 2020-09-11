using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
public class KeyValueItem : MonoBehaviour
{
    [SerializeField] Text keyText; 
    [SerializeField] Text valueText;
    
    public string Key { get; private set; }

    public void Init(string key)
    {
        Key = key;
        keyText.text = Key;
    }

    public void SetValueText(string text)
    {
        valueText.text = text;
    }
}

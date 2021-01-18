using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuButtonValue : MenuButton
{

    [SerializeField] TextMeshProUGUI valueText; 

    private List<MenuButtonItemValue> list = new List<MenuButtonItemValue>();
    private MenuButtonItemValue currentValue;

    public void SetClickListener(Action<int> callBack)
    {
        Button.onClick.AddListener(() => OnButtonClick(callBack));
    }

    private void OnButtonClick(Action<int> callBack)
    {
        if (currentValue == null)
            currentValue = list[0];

        int index = list.IndexOf(currentValue);
        index++;
        if (index > list.Count - 1)
            index = 0;
        currentValue = list[index];
        valueText.text = currentValue.text;
        callBack(index);
    }

    public void SetButtonList(List<MenuButtonItemValue> list)
    {
        this.list = list;
        if (this.list.Count > 0)
        {
            currentValue = list[0];
            valueText.text = currentValue.text;
        }
    }

    public class MenuButtonItemValue
    {
        public string text;

        public MenuButtonItemValue(string text)
        {
            this.text = text;
        }
    }
}

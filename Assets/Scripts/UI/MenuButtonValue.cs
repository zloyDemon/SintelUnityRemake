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
    private int currentIndex;

    public List<MenuButtonItemValue> ListValues => list;

    public void SetClickListener(Action<int> callBack)
    {
        Button.onClick.AddListener(() => OnButtonClick(callBack));
    }

    private void OnButtonClick(Action<int> callBack)
    {
        if (currentIndex == -1)
            currentIndex = 0;

        currentIndex++;
        if (currentIndex > list.Count - 1)
            currentIndex = 0;

        valueText.text = list[currentIndex].text;
        callBack(currentIndex);
    }

    public void SetButtonList(List<MenuButtonItemValue> list)
    {
        this.list = list;
    }

    public void SelectValueByIndex(int index)
    {
        if (index > (list.Count - 1))
        {
            throw new Exception("Index bigger than list count");
        }

        currentIndex = index;
        valueText.text = list[currentIndex].text;
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

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcHpBar : UiForGO
{
    [SerializeField] Image hpIndicator;

    private float maxWidth;
    private CharacterData characterData;

    private void Awake()
    {
        maxWidth = hpIndicator.rectTransform.rect.width;
    }

    public void SetCharacterData(CharacterData data)
    {
        characterData = data;
        characterData.OnHealthChange += OnHealthChanged;
        ActiveHPBar(true);
        SetValue((float)characterData.Health / characterData.MaxHealth);
    }

    public void DeleteCharacterData()
    {
        characterData.OnHealthChange -= OnHealthChanged;
        characterData = null;
        ActiveHPBar(false);
    }

    private void OnHealthChanged(int oldValue, int newValue)
    {
        if (characterData == null)
            return;

        SetValue((float)newValue / characterData.MaxHealth);
    }

    public void SetValue(float value)
    {
        value = Mathf.Clamp01(value);
        var newWidth = (maxWidth * value);
        hpIndicator.rectTransform.sizeDelta = new Vector2(newWidth, hpIndicator.rectTransform.rect.height);
    }

    public void ActiveHPBar(bool active)
    {
        gameObject.SetActive(active);
    }
}

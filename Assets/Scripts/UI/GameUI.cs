using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class GameUI : MonoBehaviour
{
    [SerializeField] Canvas gameCanvas;
    [SerializeField] Image hpHit;
    [SerializeField] Image hpBar;
    [SerializeField] Image compassView;
    [SerializeField] NpcUiController npcUiController;
    [SerializeField] GameObjecUIController gameObjecUIController;

    public Canvas GameCanvas => gameCanvas;
    public GameObjecUIController GameObjecUIController  => gameObjecUIController;

    private CanvasGroup hpHitCG;
    private CharacterData characterData;

    private void Awake()
    {
        hpHitCG = hpHit.GetComponent<CanvasGroup>();
        hpHitCG.alpha = 0;
    }

    private void OnDestroy()
    {
        if(characterData != null)
            characterData.OnHealthChange += OnCharacterHeathChanger;
    }

    public void Init(CharacterData data)
    {
        characterData = data;
        characterData.OnHealthChange += OnCharacterHeathChanger;
        SetHpBarValue(characterData.Health);
    }

    private void OnCharacterHeathChanger(int oldHealthValue, int newHealthValue)
    {
        if (newHealthValue > oldHealthValue)
            Rise();
        else
            Damage();
    }

    private void SetHpBarValue(float value)
    {
        value = Mathf.Clamp01(value);
        hpBar.fillAmount = value;
    }

    private void CheckHpBarColor()
    {
        int health = characterData.Health;
        if (health < 25)
            hpBar.color = Color.red;
        else
            hpBar.color = Color.green;
    }

    private void Damage()
    {
        SetHpBarValue((float)characterData.Health / characterData.MaxHealth);
        CheckHpBarColor();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(hpHitCG.DOFade(1, 0.5f));
        sequence.Append(hpHitCG.DOFade(0, 0.5f));
        sequence.Play();
    }

    private void Rise()
    {
        var value = (float)characterData.Health / characterData.MaxHealth;
        hpBar.DOFillAmount(value, 1f).OnUpdate(() => CheckHpBarColor()).Play();
    }
}

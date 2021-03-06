﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private const string MusicTheme_1 = "sintel_menu_2";
    private float BGScaleDuration = 50f;

    [SerializeField] Image gameMenuLogo;
    [SerializeField] Image backGround;
    [SerializeField] MenuPanel mainMenuPanel;
    [SerializeField] MenuPanel settingMenu;

    private Sequence sequence;

    private void Awake()
    {
        AnimateBg();
        SoundManager.Instance.PlayNewMusic(MusicTheme_1);
    }

    private void AnimateBg()
    {
        sequence = DOTween.Sequence();
        sequence.Append(backGround.transform.DOScale(Vector2.one * 1.2f, BGScaleDuration));
        sequence.Append(backGround.transform.DOScale(Vector2.one, BGScaleDuration));
        sequence.SetLoops(-1);
        sequence.Play();
    }

    private void OnDestroy()
    {
        TweenUtils.KillAndNull(ref sequence);
    }
}

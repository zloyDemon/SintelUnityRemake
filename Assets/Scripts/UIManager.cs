using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIManager : MonoBehaviour
{
    private const float FadeImageDuration = 1f;

    public static UIManager Instance { get; private set; }

    [SerializeField] Image fadeImage;

    private Tween fadeTween;

    private void Awake()
    {
        if (Instance == this)
            Destroy(gameObject);
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);
        SetImageBlack(true);
        FadeOut();
    }

    public void FadeIn(Action onComplete = null)
    {
        FadeImage(1, onComplete);
    }

    public void FadeOut(Action onComplete = null)
    {
        FadeImage(0, onComplete);
    }

    private void FadeImage(float endValue, Action onComplete)
    {
        if (fadeTween != null)
        {
            fadeTween.Kill();
            fadeTween = null;
        }

        var fadeImageColor = fadeImage.color;
        fadeTween = DOTween.To(() => fadeImageColor.a, alpha => 
        {
            fadeImageColor.a = alpha;
            fadeImage.color = fadeImageColor;
        }, endValue, FadeImageDuration).OnComplete(() => onComplete?.Invoke());
    }

    public void SetImageBlack(bool isBlack)
    {
        var colorImage = fadeImage.color;
        colorImage.a = isBlack ? 1 : 0;
        fadeImage.color = colorImage;
    }
}

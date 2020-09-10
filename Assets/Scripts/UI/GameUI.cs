using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] Image hpHit;
    [SerializeField] Image hpBar;

    private CanvasGroup hpHitCG;

    private void Awake()
    {
        hpHitCG = hpHit.GetComponent<CanvasGroup>();
        hpHitCG.alpha = 0;
    }

    private void SetHpBarValue(float value)
    {
        value = Mathf.Clamp01(value);
        hpBar.fillAmount = value;
    }

    private void ApplyHitImage()
    {

    }

    IEnumerator CorApplyHitImage()
    {
        hpHitCG.alpha = 1;
        yield return new WaitForSeconds(1);
        hpHitCG.alpha = 0;
    }
}

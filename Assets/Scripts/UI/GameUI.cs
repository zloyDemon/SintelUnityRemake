using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    [SerializeField] Canvas gameCanvas;
    [SerializeField] Image hpHit;
    [SerializeField] Image hpBar;
    [SerializeField] Image compassView;

    public Canvas GameCanvas => gameCanvas;

    private CanvasGroup hpHitCG;

    private int maxHp = 100;
    private int hp;

    private void Awake()
    {
        hpHitCG = hpHit.GetComponent<CanvasGroup>();
        hpHitCG.alpha = 0;
        hp = maxHp;
        SetHpBarValue(1);
    }

    private void SetHpBarValue(float value)
    {
        value = Mathf.Clamp01(value);
        hpBar.fillAmount = value;
    }

    private void CheckHpBarColor()
    {
        if (hp < 25)
            hpBar.color = Color.red;
        else
            hpBar.color = Color.green;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            Damage();
        if (Input.GetKeyDown(KeyCode.J))
            Rise();
    }

    private void Damage()
    {
        hp -= 5;
        SetHpBarValue((float)hp / maxHp);
        CheckHpBarColor();
        Debug.Log("Damage");
        Sequence sequence = DOTween.Sequence();
        sequence.Append(hpHitCG.DOFade(1, 0.5f));
        sequence.Append(hpHitCG.DOFade(0, 0.5f));
        sequence.Play();

        
    }

    private void Rise()
    {
        hpBar.DOFillAmount(1, 1f).OnUpdate(() => CheckHpBarColor()).Play();
        hp = maxHp;
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

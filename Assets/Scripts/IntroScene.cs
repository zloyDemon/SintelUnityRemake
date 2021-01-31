using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroScene : SintelTLDirector
{
    [SerializeField] TextMeshProUGUI author;
    [SerializeField] TextMeshProUGUI basedOn;
    [SerializeField] Image logo;

    [Header("Logos in different labguage")]
    [SerializeField] Sprite rusLogo;
    [SerializeField] Sprite engLogo;

    public override void Init()
    {
        base.Init();
        author.text = LocalizationManager.GetString("intro.author");
        basedOn.text = LocalizationManager.GetString("intro.based");

        var lang = LocalizationManager.Instnance.CurrentLanguage;

        switch (lang)
        {
            case LocalizationManager.Language.English:
                logo.sprite = engLogo;
                break;
            case LocalizationManager.Language.Russian:
                logo.sprite = rusLogo;
                break;
        }
    }
}
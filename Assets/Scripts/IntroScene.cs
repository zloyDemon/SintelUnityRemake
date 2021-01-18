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
        author.text = "Марат Якушев\nпредставляет.";
        basedOn.text = "Основано на оргиниальной игре\nкомпании Blender foundation.";
    }
}

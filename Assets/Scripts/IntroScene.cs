using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScene : SintelTLDirector
{
    [SerializeField] TextMeshProUGUI author;
    [SerializeField] TextMeshProUGUI basedOn;
    [SerializeField] Image logo;

    [Header("Logos in different labguage")]
    [SerializeField] Sprite rusLogo;
    [SerializeField] Sprite engLogo;

    private PlayableDirector director;

    public override void Init()
    {
        base.Init();
        author.text = "Марат Якушев\nпредставляет.";
        basedOn.text = "Основано на оргиниальной игре\nкомпании Blender foundation.";
    }
}

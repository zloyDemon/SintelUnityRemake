using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuPanel : MenuPanel
{
    [SerializeField] RectTransform header;
    /*[SerializeField] MenuButton resolutionButton;
    [SerializeField] MenuButton postEffectButton;
    [SerializeField] MenuButton ssaoButton;
    [SerializeField] MenuButton levelOfDetails;
    [SerializeField] MenuButton difficultyButton;
    [SerializeField] MenuButton uiScaleButton;*/
    [SerializeField] MenuButtonValue fullscreenButton;
    [SerializeField] MenuButtonValue languageButton;
    [SerializeField] MenuButton returnAndDiscardButton;
    [SerializeField] MenuButton returnAndSaveButton;

    private string headerText = "Some settings require a game restart";

    /*
Some settings require a game restart

Resolution 1600x900
Fullscreen* false : bool
Post Effects false : bool
SSAO false : bool
Level of detail low : list-> Low Medium High
Difficulty Casual : list -> Casual Easy Medium Hard
UI Scale Small : list -> Small Normal Large

Return & Discard Changes
Return & Save Changes
*/

    public override void Awake()
    {
        base.Awake();
        header.GetComponentInChildren<SintelUIText>().Text = headerText;
        InitButtons();
    }

    private void InitButtons()
    {
        fullscreenButton.SetButtonList(new List<MenuButtonValue.MenuButtonItemValue> 
        {
            new MenuButtonValue.MenuButtonItemValue("No"),
            new MenuButtonValue.MenuButtonItemValue("Yes"),
        });

        languageButton.SetButtonList(new List<MenuButtonValue.MenuButtonItemValue>
        {
            new MenuButtonValue.MenuButtonItemValue("Русский"),
            new MenuButtonValue.MenuButtonItemValue("English"),
        });

        fullscreenButton.Text = "Fullscreen";
        languageButton.Text = "Language";
        returnAndDiscardButton.Text = "Return and discard changes";
        returnAndSaveButton.Text = "Return and save changes";

        fullscreenButton.SetClickListener(FullScreenButtonClick);
        languageButton.SetClickListener(LanguageButtonClick);
        returnAndSaveButton.SetClickListener(() => Close());
    }

    private void FullScreenButtonClick(int value)
    {
        Debug.Log($"Fullscreen {value > 0}");
    }

    private void LanguageButtonClick(int value)
    {
        string[] languages = { "Русский", "English" };
        Debug.Log($"Language {languages[value]}");
    }

    public override void CollectPanelElementsToList()
    {
        PanelElements.Add(header);
        /*PanelElements.Add(resolutionButton.transform);
        PanelElements.Add(fullscreenButton.transform);
        PanelElements.Add(postEffectButton.transform);
        PanelElements.Add(ssaoButton.transform);
        PanelElements.Add(levelOfDetails.transform);
        PanelElements.Add(difficultyButton.transform);
        PanelElements.Add(uiScaleButton.transform);*/
        PanelElements.Add(languageButton.transform);
        PanelElements.Add(fullscreenButton.transform);
        PanelElements.Add(returnAndDiscardButton.transform);
        PanelElements.Add(returnAndSaveButton.transform);
    }
}

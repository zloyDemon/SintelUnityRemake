using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuPanel : MenuPanel
{
    [SerializeField] RectTransform header;
    [SerializeField] MenuButtonValue fullscreenButton;
    [SerializeField] MenuButtonValue languageButton;
    [SerializeField] MenuButton returnAndDiscardButton;
    [SerializeField] MenuButton returnAndSaveButton;

    public override void Awake()
    {
        base.Awake();
        InitButtons();
        LocalizationManager.Instnance.OnGameLocalizationChanged += UpdateLocalization;
    }

    private void OnDestroy()
    {
        LocalizationManager.Instnance.OnGameLocalizationChanged -= UpdateLocalization;
    }

    private void InitButtons()
    {
        InitButtonsText();
        languageButton.SelectValueByIndex((int)LocalizationManager.Instnance.CurrentLanguage);
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
        LocalizationManager.Instnance.SetLanguage((LocalizationManager.Language)value);
    }

    public override void CollectPanelElementsToList()
    {
        PanelElements.Add(header);
        PanelElements.Add(languageButton.transform);
        PanelElements.Add(fullscreenButton.transform);
        PanelElements.Add(returnAndDiscardButton.transform);
        PanelElements.Add(returnAndSaveButton.transform);
    }

    private void UpdateLocalization(LocalizationManager.Language lang)
    {
        InitButtonsText();
    }

    private void InitButtonsText()
    {
        fullscreenButton.SetButtonList(new List<MenuButtonValue.MenuButtonItemValue>
        {
            new MenuButtonValue.MenuButtonItemValue(LocalizationManager.GetString("gui.game.no")),
            new MenuButtonValue.MenuButtonItemValue(LocalizationManager.GetString("gui.game.yes")),
        });

        languageButton.SetButtonList(new List<MenuButtonValue.MenuButtonItemValue>
        {
            new MenuButtonValue.MenuButtonItemValue(LocalizationManager.GetString("gui.menu.options.language.en")),
            new MenuButtonValue.MenuButtonItemValue(LocalizationManager.GetString("gui.menu.options.language.ru")),
        });

        header.GetComponentInChildren<SintelUIText>().Text = LocalizationManager.GetString("gui.game.options.textOnTop");
        fullscreenButton.Text = LocalizationManager.GetString("gui.game.options.fullScreen");
        languageButton.Text = LocalizationManager.GetString("gui.game.options.language");
        returnAndDiscardButton.Text = LocalizationManager.GetString("gui.game.options.returnAndDiscard");
        returnAndSaveButton.Text = LocalizationManager.GetString("gui.game.options.returnAndSave");
    }
}

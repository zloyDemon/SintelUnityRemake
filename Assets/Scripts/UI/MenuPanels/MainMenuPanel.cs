using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPanel : MenuPanel
{
    [SerializeField] MenuButton newGameButton;
    [SerializeField] MenuButton optionsButton;
    [SerializeField] MenuButton exitButton;

    [SerializeField] SettingsMenuPanel settingsMenuPanel;

    public override void Awake()
    {
        base.Awake();

        InitButtonsText();
        newGameButton.SetClickListener(OnNewGameButtonClick);
        optionsButton.SetClickListener(OnOptionsButtonClick);
        exitButton.SetClickListener(OnExitButtonClick);
        LocalizationManager.Instnance.OnGameLocalizationChanged += UpdateLocalization;
        Open(null, null);
    }

    private void OnDestroy()
    {
        LocalizationManager.Instnance.OnGameLocalizationChanged -= UpdateLocalization;
    }

    private void UpdateLocalization(LocalizationManager.Language lang)
    {
        InitButtonsText();
    }

    private void InitButtonsText()
    {
        newGameButton.Text = LocalizationManager.GetString("gui.menu.newgame");
        optionsButton.Text = LocalizationManager.GetString("gui.menu.options");
        exitButton.Text = LocalizationManager.GetString("gui.game.exit");
    }

    private void OnNewGameButtonClick()
    {
        SintelGame.Instance.Loader.LoadScene("docks_level");
    }

    private void OnOptionsButtonClick()
    {
        OpenSettingsMenuPanel(optionsButton.transform);
    }

    private void OnExitButtonClick()
    {
        
    }

    public void OpenSettingsMenuPanel(Transform button)
    {
        settingsMenuPanel.Open(this, button);
    }

    private void PackElementsToList()
    {
        PanelElements.Add(newGameButton.transform);
        PanelElements.Add(optionsButton.transform);
        PanelElements.Add(exitButton.transform);
    }


    public override void CollectPanelElementsToList()
    {
        PackElementsToList();
    }
}
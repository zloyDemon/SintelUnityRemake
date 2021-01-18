using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MenuPanel
{
    [SerializeField] MenuButton newGameButton;
    [SerializeField] MenuButton optionsButton;
    [SerializeField] MenuButton exitButton;

    [SerializeField] SettingsMenuPanel settingsMenuPanel;

    public override void Awake()
    {
        base.Awake();   

        newGameButton.Text = "New game";
        optionsButton.Text = "Options";
        exitButton.Text = "Exit";

        newGameButton.SetClickListener(OnNewGameButtonClick);
        optionsButton.SetClickListener(OnOptionsButtonClick);
        exitButton.SetClickListener(OnExitButtonClick);

        Open(null, null);
    }

    private void OnNewGameButtonClick()
    {
        LoadLevel("docks_level");
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

    private void LoadLevel(string levelName)
    {
        StartCoroutine(CorLoadScene(levelName));
    }

    private void PackElementsToList()
    {
        PanelElements.Add(newGameButton.transform);
        PanelElements.Add(optionsButton.transform);
        PanelElements.Add(exitButton.transform);
    }

    IEnumerator CorLoadScene(string name)
    {
        var loadingScene = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        while (loadingScene != null && !loadingScene.isDone)
            yield return new WaitForEndOfFrame();

        yield return new WaitForEndOfFrame();
    }

    public override void CollectPanelElementsToList()
    {
        PackElementsToList();
    }
}
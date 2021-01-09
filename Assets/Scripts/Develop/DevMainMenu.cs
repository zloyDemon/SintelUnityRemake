using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevMainMenu : MonoBehaviour
{
    [SerializeField] Button dockButton;
    [SerializeField] Button devButton;
    [SerializeField] Button introScene;
    [SerializeField] SintelTLDirector introTimeline;

    private void Awake()
    {
        dockButton.onClick.AddListener(() => LoadIntroTL());
        devButton.onClick.AddListener(() => LoadLevel("develop_scene"));
        introScene.onClick.AddListener(() => LoadLevel("test_scene"));
    }

    private void LoadLevel(string levelName)
    {
        StartCoroutine(CorLoadScene(levelName));
    }

    private void LoadIntroTL()
    {
        introTimeline.gameObject.SetActive(true);
        introTimeline.PlayTimeline();
        introTimeline.OnTimelinePlayed += OnIntroPlayed;
    }

    private void OnIntroPlayed(PlayableDirector d)
    {
        introTimeline.OnTimelinePlayed -= OnIntroPlayed;
        LoadLevel("docks_level");
    }

    IEnumerator CorLoadScene(string name)
    {
        var loadingScene = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        while (!loadingScene.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
            
        yield return new WaitForEndOfFrame();
    }
}

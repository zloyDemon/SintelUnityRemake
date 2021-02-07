using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public enum LoadSceneType
    {
        Soft,
        Hard,
    }

    private LoadingScreen loadingScreen;

    private void Awake()
    {
        loadingScreen = UIManager.Instance.LoadingScreen;    
    }

    public event Action SceneLoaded = () => { };

    public void LoadScene(string sceneName)
    {
        UIManager.Instance.FadeIn(() => 
        {
            loadingScreen.gameObject.SetActive(true);
            UIManager.Instance.SetImageBlack(false);
            StartCoroutine(CorLoadScene(sceneName));
        });
    }

    public void LevelLoaded()
    {
        loadingScreen.gameObject.SetActive(false);
        UIManager.Instance.SetImageBlack(true);
        UIManager.Instance.FadeOut();
    }

    private IEnumerator CorLoadScene(string sceneName)
    {
        var sceneInfo = SceneManager.LoadSceneAsync(sceneName);
        while (!sceneInfo.isDone)
            yield return new WaitForEndOfFrame();

        SceneLoaded();
    }
}
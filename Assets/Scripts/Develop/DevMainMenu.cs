using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevMainMenu : MonoBehaviour
{
    [SerializeField] Button dockButton;
    [SerializeField] Button devButton;

    private void Awake()
    {
        dockButton.onClick.AddListener(() => LoadLevel("docks_level"));
        devButton.onClick.AddListener(() => LoadLevel("develop_scene"));
    }

    private void LoadLevel(string levelName)
    {
        //StartCoroutine(CorLoadScene(levelName));
        SceneManager.LoadScene(levelName);
    }

    IEnumerator CorLoadScene(string name)
    {
        var loadingScene = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        while (!loadingScene.isDone)
            yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Starter : MonoBehaviour
{

    public void Awake()
    {
        StartCoroutine(CorStart());
    }

    IEnumerator CorStart()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("menu_scene");
    }
}

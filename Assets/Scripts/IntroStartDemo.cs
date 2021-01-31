using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class IntroStartDemo : MonoBehaviour
{
    [SerializeField] SintelTLDirector introTl;

    SintelTLDirector introTLDirector;

    private TriggerGO triggerGO;

    private void Awake()
    {
        triggerGO = GetComponent<TriggerGO>();
        
        introTLDirector = Instantiate(introTl);
        triggerGO.OnTriggerEntered += OnTriggerEnetered;
    }

    private void OnDestroy()
    {
        triggerGO.OnTriggerEntered -= OnTriggerEnetered;
    }

    private void OnTriggerEnetered(Collider collider)
    {
        introTLDirector.PlayTimeline();
        introTLDirector.OnTimelinePlayed += OnTimeLinePlayed;
    }

    private void OnTimeLinePlayed(PlayableDirector pd)
    {
        introTLDirector.OnTimelinePlayed -= OnTimeLinePlayed;
        SceneManager.LoadScene("menu_scene");
    }
}

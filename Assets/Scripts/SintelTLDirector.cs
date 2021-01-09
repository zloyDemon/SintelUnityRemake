using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SintelTLDirector : MonoBehaviour
{
    private PlayableDirector director;

    public event Action<PlayableDirector> OnTimelinePlayed = d => { };

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        Init();
    }

    public virtual void Init()
    {

    }

    public void PlayTimeline()
    {
        if (!director.gameObject.activeSelf)
            director.gameObject.SetActive(true);
        director.Play();
        director.played += OnTimelindePlayed;
    }

    private void OnTimelindePlayed(PlayableDirector d)
    {
        OnTimelinePlayed(d);
    }
}

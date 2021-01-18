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
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        director.Play();
        director.stopped += OnTimelindePlayed;
    }

    private void OnTimelindePlayed(PlayableDirector d)
    {
        director.played -= OnTimelindePlayed;
        OnTimelinePlayed(d);
    }
}

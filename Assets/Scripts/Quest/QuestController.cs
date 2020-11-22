using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SintelQuest;

public class QuestController : MonoBehaviour
{
    [SerializeField] List<SintelQuest> quests;

    private QuestGameUIView questGameUIView;

    public SintelQuest CurrentQuest { get; private set; }

    public event Action<SintelQuest> CurrentQuestChanged = sq => { };

    public void Awake()
    {
        SintelGameManager.Instance.OnLevelLoaded += OnLevelLoaded;
    }

    private void OnDestroy()
    {
        if (CurrentQuest != null)
        {
            CurrentQuest.QuestCompleted -= OnQuestCompleted;
        }
    }

    private void OnLevelLoaded()
    {
        questGameUIView = SintelGameManager.Instance.GameUI.QuestGameUIView;
        InitQuest();
    }

    public void StartQuest<T>() where T : SintelQuest
    {
        
    }

    public void SetCurrentQuest(SintelQuest sintelQuest)
    {
        CheckForClearCurrentQuest();
        CurrentQuest = sintelQuest;
        CurrentQuest.Init();
        CurrentQuest.QuestCompleted += OnQuestCompleted;
        CurrentQuestChanged(CurrentQuest);
    }

    private void InitQuest()
    {
        var quest = quests[0];
        var initedQuest = Instantiate(quest);
        SetCurrentQuest(initedQuest);
    }

    private void CheckForClearCurrentQuest()
    {
        if (CurrentQuest == null)
            return;
    }

    private void OnQuestCompleted(SintelQuest sintelQuest)
    {
        if (sintelQuest == CurrentQuest)
            CurrentQuest.QuestCompleted -= OnQuestCompleted;

        sintelQuest.gameObject.SetActive(false);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static SintelQuest;

public class QuestGameUIView : MonoBehaviour
{
    private const float ObjectivePointOffsetUp = 30f;

    [Header("Quest info")]
    [SerializeField] Transform _questInfoHolder;
    [SerializeField] Text _mainQuestTitle;
    [SerializeField] Text _currentSubQuestText;
    [Header("Quest completed label")]
    [SerializeField] CanvasGroup _questLabelHolder;
    [SerializeField] Text _questCompletedLabel;
    [SerializeField] Text _questCompletedName;

    private SintelQuest _currentSintelQuest;
    private ObjectivePoint _currentObjectivePoint;
    private GameObjecUIController _gameObjecUIController;
    private GameObject firstTarget;

    private void Awake()
    {
        SintelGameManager.Instance.QuestController.CurrentQuestChanged += OnQuestChanged;
        SintelGameManager.Instance.OnLevelLoaded += OnLevelLoaded;
    }

    private void Start()
    {
        _gameObjecUIController = SintelGameManager.Instance.GameUI.GameObjecUIController;
        _questLabelHolder.alpha = 0;
        _questCompletedLabel.text = "Quest completed";
        _currentObjectivePoint = _gameObjecUIController.RequestView<ObjectivePoint>();
    }

    private void OnLevelLoaded()
    {
        
    }

    private void OnDestroy()
    {
        SintelGameManager.Instance.QuestController.CurrentQuestChanged -= OnQuestChanged;
        ClearCurrentQuest();
        SintelGameManager.Instance.OnLevelLoaded -= OnLevelLoaded;
    }

    private void OnQuestChanged(SintelQuest sintelQuest)
    {
        ClearCurrentQuest();
        _currentSintelQuest = sintelQuest;
        _currentSintelQuest.SubQuest_Changed += OnSubQuestChanged;
        _currentSintelQuest.QuestCompleted += OnQuestCompleted;
        _mainQuestTitle.text = _currentSintelQuest.QuestTitle;
        _currentObjectivePoint.gameObject.SetActive(true);
        OnSubQuestChanged(null, _currentSintelQuest.CurrentSubQuest);
    }

    private void OnSubQuestChanged(SubQuest oldSubQuest, SubQuest newSubQuest)
    {
        if (oldSubQuest != null)
        {
            oldSubQuest.TargetCompleted -= OnSubQuestTargetComplete;
        }

        newSubQuest.TargetCompleted += OnSubQuestTargetComplete;
        firstTarget = newSubQuest.Targets[0];
        _currentSubQuestText.text = newSubQuest.Task;
        _currentObjectivePoint.Init(firstTarget.transform);
        _currentObjectivePoint.GetComponent<FollowGOView>().SetFollowTarget(firstTarget);
        _currentObjectivePoint.GetComponent<FollowGOView>().SetOffset(0, ObjectivePointOffsetUp);

        if (newSubQuest.Targets.Count > 1)
        {
            for(int i = 1; i < newSubQuest.Targets.Count; i++)
            {
                var target = newSubQuest.Targets[i].transform;
                _gameObjecUIController.RequestView<ObjectivePoint>(target, op => op.Init(target), Vector3.up * ObjectivePointOffsetUp);
            }
        }
    }

    private void OnQuestCompleted(SintelQuest sintelQuest)
    {
        _questCompletedName.text = sintelQuest.QuestTitle;
        _questInfoHolder.gameObject.SetActive(false);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_questLabelHolder.DOFade(1, 2f));
        sequence.AppendInterval(3);
        sequence.Append(_questLabelHolder.DOFade(0, 2f));
        sequence.Play();
    }

    private void OnSubQuestTargetComplete(GameObject target)
    {
        if (target == firstTarget)
        {
            _currentObjectivePoint.GetComponent<FollowGOView>().SetFollowTarget(null);
            return;
        }

        _gameObjecUIController.DisableView<ObjectivePoint>(target.transform);
    }

    private void ClearCurrentQuest()
    {
        if (_currentSintelQuest == null)
            return;

        _currentSintelQuest.SubQuest_Changed -= OnSubQuestChanged;
        _currentSintelQuest.QuestCompleted -= OnQuestCompleted;
        _currentSintelQuest = null;
    }
}
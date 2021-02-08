using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SintelQuest : MonoBehaviour, IDisposable
{
    string questTitle;
    int questId;
    private SubQuest currentSubQuest;
    private Queue<SubQuest> subQuests = new Queue<SubQuest>();

    public string QuestTitle => questTitle;
    public SubQuest CurrentSubQuest => currentSubQuest;
    public int QuestId => questId;

    public event Action<SubQuest> SubQuestChanged = (sqn) => { };
    public event Action<SubQuest, SubQuest> SubQuest_Changed = (sqo, sqn) => { };
    public event Action<SintelQuest> QuestCompleted = sq => { };

    public virtual void Init()
    {
        subQuests = CreateSubQuest();
        NextSubQuest();
    }

    protected void SetSubtitleText(string text)
    {
        SintelGameManager.Instance.GameUI.SetSubtitleText(text);
    }

    protected void SetQuestData(int id, string title)
    {
        this.questId = id;
        this.questTitle = title;
    }

    public void PutSubQuestInQueue(SubQuest subQuest)
    {
        subQuests.Enqueue(subQuest);
    }

    public void SubQuestTargetComplete(GameObject oldTarget)
    {
        CurrentSubQuest.TargetComplete(oldTarget);
        if (CurrentSubQuest.Targets.Count == 0)
            NextSubQuest();
    }

    protected void NextSubQuest()
    {
        if (subQuests.Count == 0)
        {
            QuestCompleted(this);
            currentSubQuest.DisposeSQuest();
            currentSubQuest = null;
            Dispose();
            return;
        }

        SubQuest cloneSubQuest = null;

        if (currentSubQuest != null)
        {
            currentSubQuest.DisposeSQuest();
            cloneSubQuest = currentSubQuest.Clone();
        }

        currentSubQuest = subQuests.Dequeue();
        currentSubQuest.InitSubquest();

        SubQuest_Changed(cloneSubQuest, currentSubQuest);
        SubQuestChanged(currentSubQuest);
    }

    abstract public Queue<SubQuest> CreateSubQuest();

    public virtual void Dispose()
    {
        
    }

    public class SubQuest
    {
        public string Task { get; set; }
        public List<GameObject> Targets { get; private set; }

        private Action InitSubQuest = () => { };
        private Action DisposeSubQuest = () => { };

        public event Action<GameObject> TargetCompleted = t => { };

        public static SubQuest CreateSubQuest()
        {
            return new SubQuest();
        }

        public SubQuest SetTargetObject(List<GameObject> targets)
        {
            Targets = targets;
            return this;
        }

        public SubQuest SetInitDelegate(Action initDelegate)
        {
            InitSubQuest = initDelegate;
            return this;
        }

        public SubQuest SetDisposeSubQuestDelegate(Action disposeQuest)
        {
            DisposeSubQuest = disposeQuest;
            return this;
        }

        public SubQuest InitSubquest()
        {
            InitSubQuest();
            return this;
        }

        public SubQuest DisposeSQuest()
        {
            DisposeSubQuest();
            return this;
        }

        public SubQuest TargetComplete(GameObject target)
        {
            Targets.Remove(target);
            TargetCompleted(target);
            return this;
        }

        public SubQuest Clone()
        {
            var clone = SubQuest.CreateSubQuest();
            clone.Task = Task;
            clone.InitSubQuest = InitSubQuest;
            clone.DisposeSubQuest = DisposeSubQuest;
            clone.Targets = Targets;
            return clone;
        }
    }
}

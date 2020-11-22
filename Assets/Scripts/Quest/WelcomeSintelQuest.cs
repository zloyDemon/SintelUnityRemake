using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeSintelQuest : SintelQuest
{
    [SerializeField] TriggerGO talkTrigger;
    [SerializeField] TriggerGO beachBugsTrigger;
    [SerializeField] Transform bugsPointSpawn;

    public override void Init()
    {
        string questTitle = "Welcome, Sintel.";
        int id = 0;
        SetQuestData(id, questTitle);
        talkTrigger.gameObject.SetActive(false);
        beachBugsTrigger.gameObject.SetActive(false);
        base.Init();
    }

    public override Queue<SubQuest> CreateSubQuest()
    {
        Queue<SubQuest> result = new Queue<SubQuest>();

        // Talk to Arya
        SubQuest talkToArya = SubQuest.CreateSubQuest();
        talkToArya.Task = "Talk to Arya.";
        var talkToAryaTarget = new List<GameObject> { talkTrigger.gameObject };
        talkToArya.SetInitDelegate(() => TalkToArya("Sintel, you need go to the dock."))
        .SetDisposeSubQuestDelegate(TalkToAryaDispose)
        .SetTargetObject(talkToAryaTarget);
        result.Enqueue(talkToArya);

        // Move to the dock
        SubQuest moveToDock = SubQuest.CreateSubQuest();
        moveToDock.Task = "Move to the dock.";
        var moveToDockTarget = new List<GameObject> { beachBugsTrigger.gameObject };
        moveToDock.SetInitDelegate(MoveToDock)
        .SetTargetObject(moveToDockTarget)
        .SetDisposeSubQuestDelegate(MoveToDockDispose);
        result.Enqueue(moveToDock);

        // Kill the bugs
        SubQuest killBugs = SubQuest.CreateSubQuest();
        killBugs.Task = "Kill the bugs.";
        var bugsTarger = CreateBugs();
        killBugs.SetTargetObject(bugsTarger);
        result.Enqueue(killBugs);

        // Return to Arya
        SubQuest returnToArya = SubQuest.CreateSubQuest();
        returnToArya.Task = "Return to Arya";
        var aryayTarget = new List<GameObject> { talkTrigger.gameObject };
        returnToArya.SetInitDelegate(() => TalkToArya("Great, Sintel. Thank you."))
        .SetDisposeSubQuestDelegate(TalkToAryaDispose)
        .SetTargetObject(aryayTarget);
        result.Enqueue(returnToArya);

        return result;
    }

    private void TalkToArya(string talkText)
    {
        talkTrigger.gameObject.SetActive(true);
        talkTrigger.OnEnterTriggerListener(c => OnTalkTriggerEnter(c, talkText));
    }

    private void TalkToAryaDispose()
    {
        talkTrigger.gameObject.SetActive(false);
        talkTrigger.Dispose();
    }

    private void OnTalkTriggerEnter(Collider other, string text)
    {
        SintelGameManager.Instance.GameUI.SetSubtitleText(text);
        SubQuestTargetComplete(talkTrigger.gameObject);
    }

    private void MoveToDock()
    {
        beachBugsTrigger.gameObject.SetActive(true);
        beachBugsTrigger.OnEnterTriggerListener(c => {
            SetSubtitleText("Bugs! I need to kill them.");
            SubQuestTargetComplete(beachBugsTrigger.gameObject);
        });
    }

    private void MoveToDockDispose()
    {
        beachBugsTrigger.gameObject.SetActive(false);
        beachBugsTrigger.Dispose();
    }

    private List<GameObject> CreateBugs()
    {
        var list = new List<GameObject>();
        var bugOne = SintelGameManager.Instance.Spawner.SpawnBug(bugsPointSpawn.position, Quaternion.identity);
        var bugTwo = SintelGameManager.Instance.Spawner.SpawnBug(bugsPointSpawn.position, Quaternion.identity);
        bugOne.OnBugDeath += OnBugDeath;
        bugTwo.OnBugDeath += OnBugDeath;
        list.Add(bugOne.UiDistanceToPlayer.gameObject);
        list.Add(bugTwo.UiDistanceToPlayer.gameObject);
        return list;
    }

    private void OnBugDeath(BugController bug)
    {
        SubQuestTargetComplete(bug.UiDistanceToPlayer.gameObject);
        bug.OnBugDeath -= OnBugDeath;
    }
}
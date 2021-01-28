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
        string questTitle = LocalizationManager.GetString("quest.welcome.title");
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
        talkToArya.Task = LocalizationManager.GetString("quest.welcome.sq.talkToArya");
        var talkToAryaTarget = new List<GameObject> { talkTrigger.gameObject };
        talkToArya.SetInitDelegate(() => TalkToArya(LocalizationManager.GetString("quest.welcome.subt.arya_dg_1")))
        .SetDisposeSubQuestDelegate(TalkToAryaDispose)
        .SetTargetObject(talkToAryaTarget);
        result.Enqueue(talkToArya);

        // Move to the dock
        SubQuest moveToDock = SubQuest.CreateSubQuest();
        moveToDock.Task = LocalizationManager.GetString("quest.welcome.sq.moveToDock");
        var moveToDockTarget = new List<GameObject> { beachBugsTrigger.gameObject };
        moveToDock.SetInitDelegate(MoveToDock)
        .SetTargetObject(moveToDockTarget)
        .SetDisposeSubQuestDelegate(MoveToDockDispose);
        result.Enqueue(moveToDock);

        // Kill the bugs
        SubQuest killBugs = SubQuest.CreateSubQuest();
        killBugs.Task = LocalizationManager.GetString("quest.welcome.sq.killBugs"); ;
        var bugsTarger = CreateBugs();
        killBugs.SetTargetObject(bugsTarger);
        result.Enqueue(killBugs);

        // Return to Arya
        SubQuest returnToArya = SubQuest.CreateSubQuest();
        returnToArya.Task = LocalizationManager.GetString("quest.welcome.sq.returnToArya"); ; ;
        var aryayTarget = new List<GameObject> { talkTrigger.gameObject };
        returnToArya.SetInitDelegate(() => TalkToArya(LocalizationManager.GetString("quest.welcome.subt.arya_dg_2")))
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
            SetSubtitleText(LocalizationManager.GetString("quest.welcome.subt.sintel_bugs_dg"));
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
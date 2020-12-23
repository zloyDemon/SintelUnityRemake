using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SintelQuest;

public class MapUIController : MonoBehaviour
{
    [SerializeField] Transform cameraMap;
    [SerializeField] SpriteRenderer mapSprite;
    [Header("MapIcons")]
    [SerializeField] SpriteRenderer playerMapIcon;
    [SerializeField] SpriteRenderer targetIcon;

    private Transform player;
    private Transform playerCamera;
    private Vector3 mapCenter;
    private SintelQuest currentQuest;

    private Dictionary<Transform, SpriteRenderer> iconsOnMap = new Dictionary<Transform, SpriteRenderer>();

    private void Awake()
    {
        SintelGameManager.Instance.OnLevelLoaded += OnLevelLoaded;
        playerCamera = Camera.main.transform;
        playerMapIcon.transform.localScale = Vector3.one * 5;
    }

    private void OnLevelLoaded()
    {
        SintelGameManager.Instance.OnLevelLoaded -= OnLevelLoaded;
        player = SintelGameManager.Instance.SintelPlayer.transform;
        PoolManager.Instance.AddToPool<SpriteRenderer>(targetIcon, 3);
        SintelGameManager.Instance.QuestController.CurrentQuestChanged += QuestChanged;
    }

    private void QuestChanged(SintelQuest quest)
    {
        if (currentQuest != null)
        {
            currentQuest.SubQuest_Changed -= OnSubQuestChanged;
            currentQuest.QuestCompleted -= OnQuestCompleter;
        }

        currentQuest = quest;
        currentQuest.SubQuest_Changed += OnSubQuestChanged;
        currentQuest.QuestCompleted += OnQuestCompleter;
        OnSubQuestChanged(null, currentQuest.CurrentSubQuest);
    }

    private void OnQuestCompleter(SintelQuest sintelQuest)
    {
        targetIcon.gameObject.SetActive(false);
    }

    private void OnSubQuestChanged(SubQuest oldSubQuest, SubQuest newSubQuest)
    {
        if (oldSubQuest != null)
            oldSubQuest.TargetCompleted -= OnSubQuestTargetComplete;

        iconsOnMap.Clear();

        newSubQuest.TargetCompleted += OnSubQuestTargetComplete;
        var targets = newSubQuest.Targets;

        AddMarkerOnMap(targets[0].transform, targetIcon);

        for (int i = 1; i < targets.Count; i++)
        {
            var targetIconPoolable = PoolManager.Instance.GetFromPool<SpriteRenderer>();
            targetIconPoolable.sprite = targetIcon.sprite;
            targetIconPoolable.size = targetIcon.size;
            targetIconPoolable.transform.SetParent(targetIcon.transform.parent);
            AddMarkerOnMap(targets[i].transform, targetIconPoolable);
        }
    }

    private void OnSubQuestTargetComplete(GameObject target)
    {
        var sprite = iconsOnMap[target.transform];
        if (sprite != targetIcon)
            PoolManager.Instance.ReturnToPool<SpriteRenderer>(sprite);
        else
            sprite.gameObject.SetActive(false);
        iconsOnMap.Remove(target.transform);
    }

    private void Update()
    {
        if (player == null)
            return;

        cameraMap.position = new Vector3(player.position.x, cameraMap.position.y, player.position.z);
        cameraMap.rotation = Quaternion.Euler(cameraMap.localEulerAngles.x, playerCamera.localEulerAngles.y, cameraMap.localEulerAngles.z);
        mapCenter = player.transform.position;
        playerMapIcon.transform.position = player.transform.position;
        playerMapIcon.transform.localRotation = Quaternion.Euler(playerMapIcon.transform.localEulerAngles.x, player.transform.localEulerAngles.y, 0);
        UpdateIconsPositionOnMap();
    }

    private void UpdateIconsPositionOnMap()
    {
        foreach(var icon in iconsOnMap)
        {
            var targetTransform = icon.Key;
            var sprite = icon.Value;
            sprite.transform.localPosition = ClampMapPosition(targetTransform.position);
            sprite.transform.rotation = Quaternion.Euler(targetIcon.transform.eulerAngles.x, cameraMap.eulerAngles.y, 0f);
        }
    }

    private Vector3 ClampMapPosition(Vector3 pos)
    {
        Vector3 result = pos;
        float radius = cameraMap.GetComponent<Camera>().orthographicSize - 2.5f;
        bool isInCircle = ((pos.x - mapCenter.x) * (pos.x - mapCenter.x) + (pos.z - mapCenter.z) * (pos.z - mapCenter.z)) <= (radius * radius);
       
        if (!isInCircle)
        {
            var direction = (pos - mapCenter).normalized;
            direction.y = 0;
            direction = mapCenter + direction * radius;
            result = direction;
        }
            
        return result;
    }

    public void AddMarkerOnMap(Transform target, SpriteRenderer icon)
    {
        icon.gameObject.SetActive(true);
        iconsOnMap.Add(target, icon);
    }
}

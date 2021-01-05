using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SintelQuest;

public class CompasView : MonoBehaviour
{
    [SerializeField] HorizontalLayoutGroup horizontalGroup;
    [SerializeField] RectTransform compassContainer;
    [Header("Compass letters")]
    [SerializeField] SintelUIText northLetter;
    [SerializeField] SintelUIText southLetter;
    [SerializeField] SintelUIText eastLetter;
    [SerializeField] SintelUIText westLetter;
    [SerializeField] SintelUIText northWestLetter;
    [SerializeField] SintelUIText northEastLetter;
    [SerializeField] SintelUIText southWestLetter;
    [SerializeField] SintelUIText southEastLetter;

    [SerializeField] Image targetIcon;

    private Vector3 iconTargetPosition;

    Transform gameCamera;
    RectTransform horizontalGroupRT;
    private float width;
    private SintelQuest currentQuest;
    private Dictionary<Transform, Vector3> letters = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Image> targetsIconsOnCompass = new Dictionary<Transform, Image>();
    private float yValue;

    private void Awake()
    {
        gameCamera = Camera.main.transform;
        horizontalGroupRT = (RectTransform)horizontalGroup.transform;
        width = horizontalGroupRT.sizeDelta.x;
        SintelGameManager.Instance.QuestController.CurrentQuestChanged += OnCurrentQuestChanged;
        PoolManager.Instance.AddToPool<Image>(targetIcon, 3, "TargetForCompass");
        yValue = targetIcon.transform.localPosition.y;
        letters.Add(northLetter.transform, Vector3.forward);
        letters.Add(southLetter.transform, Vector3.back);
        letters.Add(eastLetter.transform, Vector3.right);
        letters.Add(westLetter.transform, Vector3.left);
        letters.Add(northWestLetter.transform, new Vector3(-1, 0, 1));
        letters.Add(northEastLetter.transform, new Vector3(1, 0, 1));
        letters.Add(southWestLetter.transform, new Vector3(-1, 0, -1));
        letters.Add(southEastLetter.transform, new Vector3(1, 0, -1));
        northLetter.Text = "N";
        southLetter.Text = "S";
        eastLetter.Text = "E";
        westLetter.Text = "W";
        northWestLetter.Text = "NW";
        northEastLetter.Text = "NE";
        southWestLetter.Text = "SW";
        southEastLetter.Text = "SE";
    }

    private void Update()
    {
        foreach (var kvp in letters)
            SetPositionForLetter(kvp.Key, kvp.Value);

        foreach (var kvp in targetsIconsOnCompass)
            SetPositionForTarget(kvp.Value.transform, Vector3.zero, kvp.Key.position);
    }

    private void SetPositionForLetter(Transform letter, Vector3 additional)
    {
        var x = FindXPositionOnCompass(letter.transform, additional, gameCamera.position);
        letter.localPosition = new Vector3(x, letter.localPosition.y, 0);
    }

    private void SetPositionForTarget(Transform view, Vector3 additional, Vector3 targetPos)
    {
        float padding = (width / 2) - targetIcon.rectTransform.rect.width / 2;
        float xValue = FindXPositionOnCompass(view.transform, additional, targetPos);
        xValue = Mathf.Clamp(xValue, -padding, padding);
        view.localPosition = new Vector3(xValue, yValue, 0);
    }

    private float FindXPositionOnCompass(Transform view, Vector3 additional, Vector3 targetPos = default)
    {
        Vector3 offset = gameCamera.InverseTransformPoint(targetPos + additional);
        float angle = Mathf.Atan2(offset.x, offset.z);
        Vector3 pos = Vector3.right * (width * 2) * angle / (2f * Mathf.PI);
        return pos.x;
    }

    private void OnCurrentQuestChanged(SintelQuest sintelQuest)
    {
        if (currentQuest != null)
            currentQuest.SubQuest_Changed -= OnSubQuestChanged;

        sintelQuest.SubQuest_Changed += OnSubQuestChanged;
        currentQuest = sintelQuest;
        OnSubQuestChanged(null, sintelQuest.CurrentSubQuest);
    }

    private void OnSubQuestChanged(SubQuest oldSubQuest, SubQuest newSubQuest)
    {
        if (oldSubQuest != null)
            oldSubQuest.TargetCompleted -= OnTargetCompleted;

        targetsIconsOnCompass.Clear();
        newSubQuest.TargetCompleted += OnTargetCompleted;

        var targets = newSubQuest.Targets;
        targetsIconsOnCompass.Add(targets[0].transform, targetIcon);
        targetIcon.gameObject.SetActive(true);

        if (targets.Count > 1)
        {
            for (int i = 1; i < targets.Count; i++)
            {
                var image = PoolManager.Instance.GetFromPool<Image>();
                image.sprite = targetIcon.sprite;
                ((RectTransform)image.transform).sizeDelta = new Vector2(30, 30);
                image.transform.SetParent(compassContainer);
                image.gameObject.SetActive(true);
                targetsIconsOnCompass.Add(targets[i].transform, image);
            }
        }
    }

    private void OnTargetCompleted(GameObject target)
    {
        if (!targetsIconsOnCompass.ContainsKey(target.transform))
        {
            Debug.LogError($"TargetsIconsCollection doesn't has {target.transform}");
            return;
        }

        var image = targetsIconsOnCompass[target.transform];
        targetsIconsOnCompass.Remove(target.transform);
        image.gameObject.SetActive(false);

        if (image != targetIcon)
            PoolManager.Instance.ReturnToPool<Image>(image);
    }
}

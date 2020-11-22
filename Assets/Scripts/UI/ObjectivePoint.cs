using UnityEngine;
using UnityEngine.UI;

public class ObjectivePoint : UiForGO
{
    [SerializeField] Image objectiveIcon;
    [SerializeField] Text distanceText;

    private Transform player;
    private Transform target;
    private bool showDistance;

    public bool ShowDistance { get { return showDistance; } set { showDistance = value; distanceText.enabled = showDistance; }}

    public string DistanceText
    {
        set
        {
            distanceText.text = value;
        }
    }

    public void Init(Transform target)
    {
        this.target = target;
        player = SintelGameManager.Instance.SintelPlayer.transform;
    }

    private void Update()
    {
        var distance = CalculateDistanceFromPlayer();
        ShowDistance = distance > 10;
        if (ShowDistance && player != null && target != null)
        {
            DistanceText = ((int)distance).ToString() + "m";
        }
    }

    private float CalculateDistanceFromPlayer()
    {
        var distance = Vector3.Distance(player.position, target.position);
        return distance;
    }
}

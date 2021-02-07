using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class IntroStartDemo : MonoBehaviour
{
    [SerializeField] SintelTLDirector introTl;

    SintelTLDirector introTLDirector;

    private TriggerGO triggerGO;

    private void Awake()
    {
        triggerGO = GetComponent<TriggerGO>();
        introTLDirector = Instantiate(introTl);
        triggerGO.OnTriggerEntered += OnTriggerEnetered;
    }

    private void OnDestroy()
    {
        triggerGO.OnTriggerEntered -= OnTriggerEnetered;
    }

    private void OnTriggerEnetered(Collider collider)
    {
        UIManager.Instance.FadeIn(() =>
        {
            introTLDirector.PlayTimeline();
            introTLDirector.OnTimelinePlayed += OnTimeLinePlayed;
            UIManager.Instance.SetImageBlack(false);
        });
    }

    private void OnTimeLinePlayed(PlayableDirector pd)
    {
        introTLDirector.OnTimelinePlayed -= OnTimeLinePlayed;
        SceneManager.LoadScene("menu_scene");
    }
}

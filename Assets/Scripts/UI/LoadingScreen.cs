using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] SintelUIText loadingText;
    [SerializeField] Image loadingImage;

    private void Awake()
    {
        loadingText.Text = "Loading";
    }

    public void Init()
    {

    }
}

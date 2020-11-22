using PixelPlay.OffScreenIndicator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowGOView : MonoBehaviour
{
    [SerializeField] private float screenBoundOffset = 0f;
    [SerializeField] bool showOnOffscreen = false;
    [SerializeField] bool showOnScreen = false;
    [SerializeField] bool rotation = false;

    private GameObject target;
    private Vector3 offset;
    private Vector3 screenCentre;
    private Vector3 screenBounds;
    private Camera camera;
    private bool isFirst;

    public GameObject Target => target;

    private void Awake()
    {
        camera = Camera.main;
        screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
        screenBoundOffset = 0.9f;
        screenBounds = screenCentre * screenBoundOffset;
    }

    public void SetFollowTarget(GameObject target)
    {
        this.target = target;
        UpdatePosition();
        if (gameObject.activeSelf != (target != null))
            gameObject.SetActive(target != null);
    }

    public void SetOffset(float right, float up)
    {
        offset.x = right;
        offset.y = up;
    }

    private void Update()
    {
        UpdatePosition(); 
    }

    private void UpdatePosition()
    {
        if (target != null)
        {
            var screenPosition = OffScreenIndicatorCore.GetScreenPosition(camera, target.transform.position);

            if (OffScreenIndicatorCore.IsTargetVisible(screenPosition))
            {
                if (showOnScreen)
                {
                    screenPosition.z = 0;
                }
            }
            else
            {
                if (showOnOffscreen)
                {
                    float angle = float.MaxValue;
                    OffScreenIndicatorCore.GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, screenCentre, screenBounds);
                    if (rotation)
                        transform.localRotation  = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
                }
            }

            transform.position = screenPosition + offset;
        }
    }
}

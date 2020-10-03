using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiForGO : MonoBehaviour
{
    private Vector3 offset;

    public Transform targetToFollow { get; private set; }

    public void SetTargetTransform(Transform target, Vector3 offset = default)
    {
        targetToFollow = target;
        this.offset = offset;
        UpdateViewPosition();
    }

    private void Update()
    {
        UpdateViewPosition();
    }

    private void UpdateViewPosition()
    {
        if (targetToFollow != null)
            transform.position = Camera.main.WorldToScreenPoint(targetToFollow.position) + offset;
    }
}

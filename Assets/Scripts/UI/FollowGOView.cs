using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGOView : MonoBehaviour
{
    Transform target;
    Vector3 offset;

    public void SetFollowTarget(Transform target)
    {
        this.target = target;
        UpdatePosition();
    }

    public void SetOffset(float up, float right)
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
            transform.position = Camera.main.WorldToScreenPoint(target.position) + offset;
    }
}

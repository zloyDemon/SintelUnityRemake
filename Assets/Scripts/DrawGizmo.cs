using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    private enum FigureType
    {
        Sphere,
        Cube,
    }

    [SerializeField] FigureType figure;
    [SerializeField] Color color;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}

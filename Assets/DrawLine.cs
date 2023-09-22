using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private float startWidth = 0.025f;
    [SerializeField] private float endWidth = 0.025f;

    private Vector3 previousStartPoint;
    private Vector3 previousEndPoint;

    public Vector3 StartPoint { get; set; }
    public Vector3 EndPoint { get; set; }

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        StartPoint = startPoint;
        EndPoint = endPoint;
    }

    void Update() {
        // do not draw line if start & end points have not changed
        if (StartPoint == previousStartPoint && EndPoint == previousEndPoint) return;

        // draw line
        Vector3[] linePositions = { StartPoint, EndPoint };
        lineRenderer.positionCount = linePositions.Length;
        lineRenderer.SetPositions(linePositions);

        // save line start & end points
        previousStartPoint = StartPoint;
        previousEndPoint = EndPoint;
    }
}

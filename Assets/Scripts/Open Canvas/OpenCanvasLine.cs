using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OpenCanvasLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public List<Vector2> points;
    private int sortingOrder;
    public Color lineColor = Color.red;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        points = new List<Vector2>();

    }

    public void UpdateLine(Vector2 position)
    {
        if (points == null || points.Count == 0)
        {
            points = new List<Vector2>();
            SetPoints(position);
            return;
        }

        MyDebug.Log($"Points Len:{points.Count}");
        MyDebug.Log($"Points Last:{points.Last()}");

        if (Vector2.Distance(points.Last(), position) > 0.05f)
        {
            SetPoints(position);
        }
    }

    public void SetPoints(Vector2 point)
    {
        points.Add(point);
        if (points.Count >= 4)
        {
            List<Vector3> interpolatedPoints = new List<Vector3>();

            for (int i = 0; i < points.Count - 3; i++)
            {
                for (int j = 0; j <= 10; j++)
                {
                    float t = j / 10.0f;
                    Vector3 interpolatedPoint = CatmullRomSpline(points[i], points[i + 1], points[i + 2], points[i + 3], t);
                    interpolatedPoints.Add(interpolatedPoint);
                }
            }

            lineRenderer.positionCount = interpolatedPoints.Count;
            lineRenderer.SetPositions(interpolatedPoints.ToArray());
        }
        else
        {
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, point);
        }
    }

    Vector3 CatmullRomSpline(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        float a0 = -0.5f * t3 + t2 - 0.5f * t;
        float a1 = 1.5f * t3 - 2.5f * t2 + 1.0f;
        float a2 = -1.5f * t3 + 2.0f * t2 + 0.5f * t;
        float a3 = 0.5f * t3 - 0.5f * t2;

        return a0 * p0 + a1 * p1 + a2 * p2 + a3 * p3;
    }

    public void SetColor(Color color)
    {
        lineColor = color;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public void SetSortingOrder(int order)
    {
        sortingOrder = order;
        lineRenderer.sortingOrder = sortingOrder;
    }

    internal void SetLineWidthStartAndEnd(float lineWidth)
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }
}

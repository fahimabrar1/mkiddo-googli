using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class OpenCanvasLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public List<Vector2> points;



    public void Updateline(Vector2 position)
    {
        if (points == null || points.Count == 0)
        {
            points = new();
            SetPoints(position);
            return;
        }

        MyDebug.Log($"Points Len:{points.Count}");
        MyDebug.Log($"Points Last:{points.Last()}");

        if (Vector2.Distance(points.Last(), position) > .05f)
        {
            SetPoints(position);
        }
    }

    public void SetPoints(Vector2 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }

}

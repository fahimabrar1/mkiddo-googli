using UnityEngine;

public class SpriteLineDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public bool IsLineActive;
    public bool IsLineAvailavle;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;  // Initially, the line should not be visible
        IsLineAvailavle = true;
    }

    public void StartLine(Vector3 startPosition)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);
        IsLineActive = true;
    }

    public void UpdateLine(Vector3 currentPosition)
    {
        if (IsLineActive)
        {
            lineRenderer.SetPosition(1, currentPosition);
        }
    }

    public void EndLine(Vector3 endPosition)
    {
        if (IsLineActive)
        {
            lineRenderer.SetPosition(1, endPosition);
            IsLineActive = false;  // The line is now fixed
            IsLineAvailavle = false;  // The line is now fixed
        }
    }

    public void ResetLine()
    {
        lineRenderer.enabled = false;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
        IsLineActive = false;
        IsLineAvailavle = true;
    }
}

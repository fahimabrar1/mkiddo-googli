using UnityEngine;

public class DragAndDropDraggableObject : MonoBehaviour
{
    public bool GoToOriginalPosition;
    public bool EnableAnimaitons;
    public float distance = 2f; // Distance to move in each direction
    public float duration = 1f; // Time to complete each movement
    private MyTween myTween;
    private Vector3 originalPosition;
    public Vector3 siteTarget;

    public DraggableObject draggableObject;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        myTween = new(transform);
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        siteTarget = Vector3.zero;

        originalPosition = transform.position; // Consider this the central point

    }







    public void ReturnToOriginalPosition()
    {
        GoToOriginalPosition = false;
        draggableObject.isDragging = false;
        if (draggableObject.CanDrag)
        {
            myTween.TweenBackToPosition(originalPosition, 0.25f, () =>
            {
                Debug.Log("Tween Complete - Restored original position.");
            });
        }
    }

    public void SetToSidePosition(Vector3 target)
    {
        // Disable dragging for the matched object
        draggableObject.CanDrag = false;
        myTween.TweenBackToPosition(target, 0.5f);
    }



    public void OnMouseDownCallback()
    {
        Debug.Log("Mouse Down - Started dragging.");
        draggableObject.isDragging = true;
        GoToOriginalPosition = true;
    }



    public void OnMouseUpCallback()
    {
        Debug.Log("Mouse Up - Stopped dragging.");
        Debug.Log("GoToOriginalPosition: " + GoToOriginalPosition);
        if (GoToOriginalPosition)
            ReturnToOriginalPosition();
        else if (siteTarget != Vector3.zero)
            SetToSidePosition(siteTarget);
    }

    public void OnSetSiteBoolean(bool value)
    {
        GoToOriginalPosition = value;
    }

    public void OnSetSiteTargetVec3(Vector3 value)
    {
        siteTarget = value;
    }

}
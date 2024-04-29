using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSortingDraggableObject : MonoBehaviour
{
    public bool GoToOriginalPosition;
    public bool EnableAnimaitons;
    public float distance = 2f; // Distance to move in each direction
    public float duration = 1f; // Time to complete each movement
    private MyTween myTween;
    private Vector3 originalPosition;
    public Vector3 siteTarget;
    int loopingAnimationIndex = 0;

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

        loopingAnimationIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, 3));
        SetLoopingTween();
        originalPosition = transform.position; // Consider this the central point

    }


    private void SetLoopingTween()
    {
        if (loopingAnimationIndex == 0)
        {
            myTween.TweenUpDown(distance, duration);
        }
        else if (loopingAnimationIndex == 1)
        {
            myTween.TweenLeftRight(distance, duration);
        }
        else
        {
            myTween.TweenDiagonal(distance, duration);
        }
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
                if (EnableAnimaitons) SetLoopingTween();
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
        myTween?.StopTween(loopingAnimationIndex);
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

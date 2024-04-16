using System;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class DraggableObject : MonoBehaviour, IDragable
{
    public bool isDragging;
    public bool CanDrag;
    public bool GoToOriginalPosition;
    public DragAndDropManager.DropSide dropSide;
    public float distance = 2f; // Distance to move in each direction
    public float duration = 1f; // Time to complete each movement
    private MyTween myTween;
    private Vector3 originalPosition;
    private Vector3 offset;
    public Vector3 siteTarget;
    private Camera mainCamera;
    int loopingAnimationIndex = 0;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        myTween = new(transform);
        mainCamera = Camera.main;
    }




    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        CanDrag = true;
        GoToOriginalPosition = false;
        siteTarget = Vector3.zero;
        originalPosition = transform.position; // Consider this the central point
        loopingAnimationIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, 3));
        SetLoopingTween();
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

    void Update()
    {
        if (isDragging && CanDrag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Convert mouse position into a ray
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    offset = gameObject.transform.position - mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isDragging)
                {
                    isDragging = false;
                }
            }

            if (isDragging)
            {
                Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
                transform.position = mainCamera.ScreenToWorldPoint(newPosition) + offset;
                transform.position = new Vector3(transform.position.x, transform.position.y, 0); // Keep z constant
            }
        }
    }

    public void OnMouseDown()
    {
        Debug.Log("Mouse Down - Started dragging.");
        myTween?.StopTween(loopingAnimationIndex);
        isDragging = true;
        GoToOriginalPosition = true;
    }

    public void OnMouseUp()
    {
        Debug.Log("Mouse Up - Stopped dragging.");
        if (GoToOriginalPosition)
            ReturnToOriginalPosition();
        else if (siteTarget != Vector3.zero)
            SetToSidePosition(siteTarget);
    }

    public void ReturnToOriginalPosition()
    {
        GoToOriginalPosition = false;
        isDragging = false;
        if (CanDrag)
        {
            myTween.TweenBackToPosition(originalPosition, 0.25f, () =>
            {
                Debug.Log("Tween Complete - Restored original position.");
                SetLoopingTween();
            });
        }
    }

    public void SetToSidePosition(Vector3 target)
    {
        // Disable dragging for the matched object
        CanDrag = false;
        myTween.TweenBackToPosition(target, 0.5f);
    }
}

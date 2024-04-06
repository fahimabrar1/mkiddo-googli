using System;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class DraggableObject : MonoBehaviour, IDragabble
{
    private bool isDragging;
    public bool canDrag;
    public DragAndDropManager.DropSide dropSide;


    public float distance = 2f; // Distance to move in each direction
    public float duration = 1f; // Time to complete each movement

    private MyTween myTween = new();
    private Vector3 originalPosition;


    int loopingAnimationIndex = 0;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        canDrag = true;
        originalPosition = transform.position; // Consider this the central point
        loopingAnimationIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, 3));
        SetLoopingTween();
    }

    private void SetLoopingTween()
    {
        if (loopingAnimationIndex == 0)
        {
            myTween.TweenUpDown(transform, distance, duration);
        }
        else if (loopingAnimationIndex == 1)
        {
            myTween.TweenLeftRight(transform, distance, duration);
        }
        else
        {
            myTween.TweenDiagonal(transform, distance, duration);
        }
    }

    void Update()
    {
        if (isDragging & canDrag)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
    }

    public void OnMouseDown()
    {
        myTween.StopTween(loopingAnimationIndex);
        isDragging = true;
    }

    public void OnMouseUp()
    {
        isDragging = false;
    }
}

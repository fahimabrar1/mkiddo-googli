using System;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;


[System.Serializable]
public class DraggableObjectEvent : UnityEvent<DraggableObject> { }

public class DropContainer : MonoBehaviour
{
    public DropManager dragAndDropManager;
    public ImageSortingManager.DropSide dropSide;

    private DraggableObject lastEnteredDraggableObject;

    public DraggableObjectEvent OnTriggerEnter2DEvent;
    public UnityEvent OnTriggerExit2DEvent;

    [Header("For Drag And Drop Game")]

    public SpriteRenderer currentRenderObject;
    public GameObject combinedSprite;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.TryGetComponent(out DraggableObject draggableObject))
        {
            if (draggableObject.dropSide == dropSide)
            {
                lastEnteredDraggableObject = draggableObject;
                dragAndDropManager.OnDropObject(draggableObject, true);
                OnTriggerEnter2DEvent.Invoke(draggableObject);

            }
            else
            {
                dragAndDropManager.PlayFailedAudio();
                draggableObject.OnSetSiteBoolEvent?.Invoke(true);
            }
        }
    }


    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out DraggableObject draggableObject))
        {
            if (lastEnteredDraggableObject == draggableObject)
            {
                OnTriggerExit2DEvent?.Invoke();
                dragAndDropManager.OnCancelDropObject(draggableObject);
            }
        }
    }

    internal void ActivateWinScenatio()
    {
        currentRenderObject.enabled = false;
        combinedSprite.SetActive(true);
        combinedSprite.transform.DOScale(1.2f, 1);
    }
}

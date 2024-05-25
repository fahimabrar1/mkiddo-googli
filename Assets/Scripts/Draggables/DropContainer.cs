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
    public SpriteRenderer CombinedRenderObject;
    public GameObject combinedSprite;
    public float scaleTo = 2f;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.TryGetComponent(out DraggableObject draggableObject))
        {
            MyDebug.Log($"Game Object Enter2D Name: {other.gameObject.name}");
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
            MyDebug.Log($"Game Object Exit2D Name: {other.gameObject.name}");
            if (lastEnteredDraggableObject != null)
            {

                if (lastEnteredDraggableObject == draggableObject)
                {
                    OnTriggerExit2DEvent?.Invoke();
                    dragAndDropManager.OnCancelDropObject(draggableObject);
                }
            }
        }
    }

    internal void ActivateWinScenatio()
    {
        currentRenderObject.enabled = false;
        combinedSprite.SetActive(true);
        combinedSprite.transform.DOScale(scaleTo, 1);
    }
}

using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class DraggableObjectEvent : UnityEvent<DraggableObject> { }

public class DropContainer : MonoBehaviour
{
    public DragAndDropManager dragAndDropManager;
    public DragAndDropManager.DropSide dropSide;

    private DraggableObject lastEnteredDraggableObject;

    public DraggableObjectEvent OnTriggerEnter2DEvent;
    public UnityEvent OnTriggerExit2DEvent;

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
}

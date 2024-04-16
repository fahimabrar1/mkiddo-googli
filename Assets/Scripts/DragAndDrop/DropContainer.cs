using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropContainer : MonoBehaviour
{


    public DragAndDropManager dragAndDropManager;
    public DragAndDropManager.DropSide dropSide;
    public List<Vector3> Positions = new();

    private DraggableObject lastEnteredDraggableObject;
    private int occupiedPosition = 0;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Positions.Add(transform.GetChild(i).transform.position);
        }
    }

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
                draggableObject.siteTarget = Positions[occupiedPosition];
                occupiedPosition++;
                dragAndDropManager.OnDropObject(draggableObject, true);
            }
            else
            {
                draggableObject.GoToOriginalPosition = true;
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
                occupiedPosition--;
                dragAndDropManager.OnCancelDropObject(draggableObject);
            }
        }
    }
}

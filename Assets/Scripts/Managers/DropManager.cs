using UnityEngine;

public class DropManager : MonoBehaviour
{


    public virtual void OnDropObject(DraggableObject draggableObject, bool matched)
    { }


    public virtual void OnCancelDropObject(DraggableObject draggableObject)
    {
    }
}
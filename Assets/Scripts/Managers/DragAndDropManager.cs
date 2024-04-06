using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class DragAndDropManager : MonoBehaviour
{

    public enum DropSide
    {
        left,
        right
    }


    public DropContainer leftContainer;
    public DropContainer rightContainer;
    public List<DraggableObject> draggableObjects = new();


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        var allDraggableObjects = FindObjectsByType<DraggableObject>(FindObjectsSortMode.None);
        foreach (var Obj in allDraggableObjects)
        {
            draggableObjects.Add(Obj);
        }
    }




    public void OnDropObject(DraggableObject dragabble, bool matched)
    {
        var obj = draggableObjects.FirstOrDefault((d) => d == dragabble);

        if (matched)
        {
            dragabble.canDrag = false;

            //Todo: Do Tween and Snap to childposition 

        }
        else
        {
            //Todo: Do Tween and Snap to original position 
        }
    }
}

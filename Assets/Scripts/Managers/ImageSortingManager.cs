using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageSortingManager : DropManager
{
    // Enumeration for the two possible drop sides
    public enum DropSide
    {
        noSide,
        center,
        left,
        right,
    }

    // Reference to the left drop container
    public DropContainer leftContainer;

    // Reference to the right drop container
    public DropContainer rightContainer;

    // List of all draggable objects
    public List<DraggableObject> Draggables = new();

    // Total number of objects placed
    public int TotalObjectsPlaced { get; private set; }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Find all DraggableObject components in the scene and add them to the Draggables list
        Draggables = FindObjectsByType<DraggableObject>(FindObjectsSortMode.None).ToList();
    }

    // Method called when an object is dropped

    public override void OnDropObject(DraggableObject draggableObject, bool matched)
    {

        // If the object was not matched, return it to its original position
        if (!matched)
        {
            draggableObject.ReturnToOriginalPosition();
            return;
        }



        // Increment the total number of objects placed
        TotalObjectsPlaced++;

        draggableObject.OnSetSiteBoolEvent?.Invoke(false);

        // If all objects have been placed, trigger the game win logic
        if (TotalObjectsPlaced == Draggables.Count)
        {
            // Todo: Game Win
        }
    }




    public override void OnCancelDropObject(DraggableObject draggableObject)
    {
        // Decrement the total number of objects placed
        TotalObjectsPlaced--;
        draggableObject.OnSetSiteBoolEvent?.Invoke(true);
        draggableObject.OnSetSiteTargetVec3Event?.Invoke(Vector3.zero);
    }
}
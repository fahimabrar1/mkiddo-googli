using System.Collections.Generic;
using UnityEngine;

public class ImageSortingDropContainer : MonoBehaviour
{
    public List<Vector3> Positions = new();

    private int occupiedPosition = 0;

    // draggableObject.siteTarget = Positions[occupiedPosition];

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




    public void OnSetSiteTarget(DraggableObject draggableObject)
    {
        draggableObject.OnSetSiteTargetVec3Event?.Invoke(Positions[occupiedPosition]);
        occupiedPosition++;
    }
    public void OnReleaseTarget()
    {
        occupiedPosition--;
    }
}
using UnityEngine;

public class DragAndDropDropContainer : MonoBehaviour
{
    public GameObject winChild;
    public void OnSetSiteTarget(DraggableObject draggableObject)
    {
        draggableObject.gameObject.SetActive(false);
        winChild.SetActive(true);

    }

}
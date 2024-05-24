using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnSetSiteBooleanEvent : UnityEvent<bool> { }

[Serializable]
public class OnSetSiteTargetVectorEvent : UnityEvent<Vector3> { }

public class DraggableObject : MonoBehaviour, IDragable
{

    public bool isDragging;
    public bool CanDrag;
    public SpriteRenderer spriteRenderer;

    public ImageSortingManager.DropSide dropSide;
    public bool EnableAnimaitons;
    public float distance = 2f; // Distance to move in each direction
    public float duration = 1f; // Time to complete each movement
    private Vector3 offset;
    public Camera mainCamera;

    public OnSetSiteBooleanEvent OnSetSiteBoolEvent;
    public OnSetSiteTargetVectorEvent OnSetSiteTargetVec3Event;
    public UnityEvent OnMouseDownEvent;
    public UnityEvent OnMouseUpEvent;
    public UnityEvent OnReturnToOriginalPositionEvent;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        mainCamera = Camera.main;
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        CanDrag = true;
    }

    void Update()
    {
        if (isDragging && CanDrag)
        {
            if (Input.GetMouseButtonDown(0))
            {

                // Convert mouse position into a ray
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    offset = gameObject.transform.position - mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isDragging)
                {
                    isDragging = false;
                }
            }

            if (isDragging)
            {
                Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
                transform.position = mainCamera.ScreenToWorldPoint(newPosition) + offset;
                transform.position = new Vector3(transform.position.x, transform.position.y, 0); // Keep z constant
            }
        }
    }



    public void OnMouseDown()
    {
        MyDebug.Log("Clciked");

        OnMouseDownEvent?.Invoke();
    }

    public void OnMouseUp()
    {
        OnMouseUpEvent?.Invoke();
        OnReturnToOriginalPositionEvent?.Invoke();
    }

    public void ReturnToOriginalPosition()
    {
        OnReturnToOriginalPositionEvent?.Invoke();
    }

}

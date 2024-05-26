using UnityEngine;
using static Matching2SidesManager;

public class MatchingSide : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Matching2SidesManager manager;
    [SerializeField] private bool isMatched = false;
    [SerializeField] private SpriteLineDrawer activeLineRenderer;
    [SerializeField] private Matching2SidesManagerType matching2SidesManagerType;

    void Start()
    {
        mainCamera = Camera.main;
        manager = FindObjectOfType<Matching2SidesManager>();
    }

    void OnMouseDown()
    {
        if (!isMatched)
        {
            activeLineRenderer = manager.GetAvailableLineRenderer();
            if (activeLineRenderer != null)
            {
                activeLineRenderer.StartLine(transform.position);
            }
        }
    }

    void OnMouseDrag()
    {
        if (activeLineRenderer != null && activeLineRenderer.IsLineActive)
        {
            Vector3 currentPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            currentPos.z = 0;
            activeLineRenderer.UpdateLine(currentPos);
        }
    }

    void OnMouseUp()
    {
        if (activeLineRenderer != null && activeLineRenderer.IsLineActive)
        {
            GameObject endSprite = GetTouchedSprite();
            if (endSprite != null && endSprite.GetComponent<MatchingSide>() != null)
            {
                MatchingSide endSide = endSprite.GetComponent<MatchingSide>();
                if (!endSide.isMatched && endSide.matching2SidesManagerType != matching2SidesManagerType)
                {
                    activeLineRenderer.EndLine(endSprite.transform.position);
                    isMatched = true;
                    endSide.isMatched = true;
                }
                else
                {
                    activeLineRenderer.ResetLine();
                }
            }
            else
            {
                activeLineRenderer.ResetLine();
            }

            activeLineRenderer = null;  // Reset the active line renderer
        }
    }
    private bool IsTouchingThisSprite()
    {
        Vector3 touchPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        touchPos.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

        return hit.collider != null && hit.collider.gameObject == gameObject;
    }

    private GameObject GetTouchedSprite()
    {
        Vector3 touchPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        touchPos.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;
    public OpenCanvasLine activeLine;
    public RawImage drawingArea;

    public Camera renderCam;
    public RenderTexture renderTexture;
    private Texture2D texture2D;
    private RectTransform rectTransform;
    private BoxCollider2D drawingAreaCollider;

    void Start()
    {


        // Create a Texture2D to store the final image
        texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // Get the RectTransform of the RawImage
        rectTransform = drawingArea.GetComponent<RectTransform>();

        // Add a BoxCollider2D to the RawImage GameObject if it doesn't have one
        drawingAreaCollider = drawingArea.gameObject.GetComponent<BoxCollider2D>();
        if (drawingAreaCollider == null)
        {
            drawingAreaCollider = drawingArea.gameObject.AddComponent<BoxCollider2D>();
            UpdateColliderSize();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MyDebug.Log("On Click");
            if (IsRaycastHitTargetWithTag("CanvasBoard"))
            {
                MyDebug.Log("On Instantiate");
                GameObject newLine = Instantiate(linePrefab);
                if (newLine.TryGetComponent(out OpenCanvasLine line))
                {
                    activeLine = line;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            activeLine = null;
        }

        if (activeLine != null)
        {
            Vector2 mousePos = renderCam.ScreenToWorldPoint(Input.mousePosition);
            activeLine.Updateline(mousePos);
        }
    }

    bool IsRaycastHitTargetWithTag(string targetTag)
    {
        // Get the mouse position in world coordinates
        Vector3 mousePos = renderCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, renderCam.nearClipPlane));

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity);

        // Debugging line to visualize the raycast
        if (hit.collider != null)
        {
            Debug.DrawLine(mousePos, hit.point, Color.red, 2f);
            MyDebug.Log($"Hit: {hit.transform.name}");
        }
        else
        {
            MyDebug.Log("No hit detected");
        }

        // Return true if the hit object has the specified tag
        return hit.collider != null && hit.collider.CompareTag(targetTag);
    }


    void UpdateColliderSize()
    {
        if (drawingArea != null && drawingAreaCollider != null)
        {
            RectTransform rectTransform = drawingArea.GetComponent<RectTransform>();
            Vector2 size = rectTransform.rect.size;
            drawingAreaCollider.size = size;
        }
    }
    public void SaveRenderTextureToFile()
    {
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;

        byte[] bytes = texture2D.EncodeToPNG();
        string path = Application.persistentDataPath + "/SavedImage.png";
        File.WriteAllBytes(path, bytes);
        Debug.Log("Saved image to " + path);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;
    public OpenCanvasLine activeLine;
    public RawImage drawingArea;

    public Camera renderCam;
    public RenderTexture renderTexture;
    private Texture2D texture2D;
    private BoxCollider2D drawingAreaCollider;

    private Color lineColor;
    private int order;

    void Start()
    {


        // Create a Texture2D to store the final image
        texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // Add a BoxCollider2D to the RawImage GameObject if it doesn't have one
        drawingAreaCollider = drawingArea.gameObject.GetComponent<BoxCollider2D>();

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
                    activeLine.SetColor(lineColor);
                    activeLine.SetSortingOrder(order++);
                }
            }
        }

        if (Input.GetMouseButton(0) && !IsRaycastHitTargetWithTag("CanvasBoard"))
        {
            activeLine = null;
        }
        if (Input.GetMouseButtonUp(0) || !IsRaycastHitTargetWithTag("CanvasBoard"))
        {
            activeLine = null;
        }

        if (activeLine != null)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousePos = new(pos.x + 100, pos.y);
            activeLine.UpdateLine(mousePos);
        }
    }
    bool IsRaycastHitTargetWithTag(string targetTag)
    {
        // Get the mouse position in world coordinates
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector3 mousePos = new(pos.x + 100, pos.y, pos.z);
        // MyDebug.Log($"Mouse Pos: {mousePos}");

        // Create a ray from the mouse position in the forward direction of the camera
        Ray ray = new(mousePos, Vector3.forward);

        // Debugging line to visualize the raycast direction
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 30, Color.green, 2f);

        // Perform the raycast
        var hits = Physics.RaycastAll(ray.origin, ray.direction, 30);

        // Check if any hit has the specified tag
        foreach (var hit in hits)
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 2f);
            MyDebug.Log($"Hit: {hit.transform.name}");

            if (hit.collider != null && hit.collider.CompareTag(targetTag))
            {
                return true;
            }
        }

        // No hit detected with the specified tag
        MyDebug.Log("No hit detected");
        return false;
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

    public void SetPenColor(Color color)
    {
        lineColor = color;
    }
}

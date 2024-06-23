using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class LineGenerator : MonoBehaviour
{
    public AkibukiManager akibukiManager;
    public GameObject linePrefab;
    public OpenCanvasLine activeLine;
    public RawImage drawingArea;

    public Camera renderCam;
    public RenderTexture renderTexture;
    private Texture2D texture2D;
    public Transform parent;
    public SpritesContainerSO spritesContainerSO;

    private Color lineColor;
    private float lineWidth;
    private int order;


    private bool isStickerMode;
    private GameObject selectedSticker;

    void Start()
    {
        // Create a Texture2D to store the final image
        texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isStickerMode && IsRaycastHitTargetWithTag("CanvasBoard"))
            {
                PlaceSticker();
            }
            else
            {
                MyDebug.Log("On Click");
                if (IsRaycastHitTargetWithTag("CanvasBoard"))
                {
                    MyDebug.Log("On Instantiate");
                    GameObject newLine = Instantiate(linePrefab, parent);
                    if (newLine.TryGetComponent(out OpenCanvasLine line))
                    {
                        activeLine = line;
                        activeLine.SetColor(lineColor);
                        activeLine.SetLineWidthStartAndEnd(lineWidth);
                        activeLine.SetSortingOrder(order++);
                    }
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
        Vector3 mousePos = Input.mousePosition;

        // Ensure mouse position is within screen bounds
        if (mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height)
        {
            MyDebug.Log("Mouse position out of screen bounds");
            return false;
        }

        // Get the mouse position in world coordinates
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector3 mouseNewPos = new(pos.x + 100, pos.y, pos.z);

        // Create a ray from the mouse position in the forward direction of the camera
        Ray ray = new(mouseNewPos, Vector3.forward);

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

    internal void SetLineWidth(float val)
    {
        lineWidth = val;
    }

    // New method to handle sticker mode
    public void SelectSticker(GameObject sticker)
    {
        isStickerMode = true;
        selectedSticker = sticker;
        activeLine = null; // Deselect any active line
    }

    // New method to handle pen mode
    public void SelectPen()
    {
        isStickerMode = false;
        selectedSticker = null;
    }

    // New method to handle pen mode
    public void SwitchToStickerMode(bool toggle)
    {
        isStickerMode = toggle;
    }




    private void PlaceSticker()
    {

        Vector3 mousePos = Input.mousePosition;


        if (mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height)
        {
            MyDebug.Log("Mouse position out of screen bounds");
            return;
        }

        // Get the mouse position in world coordinates
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector3 mouseNewPos = new(pos.x + 100, pos.y, pos.z);

        var obj = new GameObject("Sticker");
        obj.transform.parent = parent;
        obj.transform.position = mouseNewPos;
        obj.AddComponent(typeof(SpriteRenderer));
        if (obj.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.sprite = akibukiManager.GetCurrentSticker();
            spriteRenderer.sortingOrder = order++;
        }
        // }
        // GameObject newSticker = Instantiate(selectedSticker, mousePos, Quaternion.identity, parent);
        // // Adjust sorting order
        // var spriteRenderer = newSticker.GetComponent<SpriteRenderer>();
        // if (spriteRenderer != null)
        // {
        //     spriteRenderer.sortingOrder = order++;
        // }

    }
}

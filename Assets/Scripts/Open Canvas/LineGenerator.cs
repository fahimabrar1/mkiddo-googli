using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;
    public OpenCanvasLine activeLine;


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newLine = Instantiate(linePrefab);
            if (newLine.TryGetComponent(out OpenCanvasLine line))
            {
                activeLine = line;
            }
        }


        if (Input.GetMouseButtonUp(0))
        {
            activeLine = null;
        }


        if (activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MyDebug.Log($"Mouse Pos:{mousePos}");
            activeLine.Updateline(mousePos);
        }
    }
}
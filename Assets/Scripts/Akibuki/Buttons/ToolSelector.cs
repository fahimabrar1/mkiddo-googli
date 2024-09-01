using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolSelector : MonoBehaviour
{
    public List<GameObject> ToolObjs;
    public List<Button> ToolButtons;
    public List<Image> ToolOutline;

    // Start is called before the first frame update
    void Start()
    {
        ToolButtons = new List<Button>();
        ToolOutline = new List<Image>();

        // Populate ToolButtons and ToolOutline
        for (int i = 0; i < ToolObjs.Count; i++)
        {
            var toolObj = ToolObjs[i];
            var outline = toolObj.GetComponent<Image>();
            var btn = toolObj.GetComponent<Button>();

            // Store the current index in a local variable
            int index = i;

            // Add the outline image and button to their respective lists
            ToolOutline.Add(outline);
            ToolButtons.Add(btn);

            // Add the listener with the local index
            btn.onClick.AddListener(() => OnSelectButton(index));
        }

        OnSelectButton(1);
    }

    private void OnSelectButton(int val)
    {
        Debug.Log($"Button clicked: {val}"); // Debug log to check which button is clicked

        for (int i = 0; i < ToolOutline.Count; i++)
        {
            // Log the index and whether it is enabled or not
            Debug.Log($"Setting ToolOutline[{i}] to {(i == val)}");

            ToolOutline[i].enabled = i == val;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic if needed
    }
}

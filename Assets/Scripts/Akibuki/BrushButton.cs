using UnityEngine;
using UnityEngine.UI;

public class BrushButton : MonoBehaviour
{
    public int id;
    public Image image;

    public ButtonSelector selector;
    public void OnClickBrush()
    {
        selector.OnBrushSelect(id);
    }


    public void OnSetBrush(int id)
    {
        Color col = image.color;
        if (this.id == id)
        {
            col.a = 1;

        }
        else
        {
            col.a = .5f;
        }
        image.color = col;
    }
}
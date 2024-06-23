using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerButton : MonoBehaviour
{
    public int id;

    public Image background;
    public Image content;
    public Sprite activeBG;
    public Sprite inActiveBG;

    public CanvasStickerManager canvasStickerManager;

    public void OnClickSticker()
    {
        canvasStickerManager.SetStickerIndex(id);
    }


    public void OnUpdateButton(int id)
    {
        if (this.id == id)
        {
            background.sprite = activeBG;
        }
        else
        {
            background.sprite = inActiveBG;
        }
    }

    public void OnResetButton()
    {
        background.sprite = inActiveBG;
    }
}

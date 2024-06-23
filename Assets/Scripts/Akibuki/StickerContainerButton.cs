using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerContainerButton : MonoBehaviour
{
    public int id;

    public Image content;
    public Button button;

    public CanvasStickerManager canvasStickerManager;

    public void OnClickSticker()
    {
        canvasStickerManager.SetSpriteContainerIndex(id);
    }

}

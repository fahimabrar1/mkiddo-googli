using System;
using UnityEngine;

public class AkibukiManager : MonoBehaviour
{
    public LineGenerator lineGenerator;
    public CanvasStickerManager canvasStickerManager;
    public CanvasAudioManager canvasAudioManager;

    public Transform Holder;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        CanDraw(true);
    }



    public void SetPenColor(Color color)
    {
        lineGenerator.SetPenColor(color);
    }

    internal void SetLineWidth(float val)
    {
        lineGenerator.SetLineWidth(val);
    }


    public void ClearCanvas()
    {
        if (Holder.childCount > 0)
        {
            int childCount = Holder.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Destroy(Holder.GetChild(i).gameObject);
            }
        }
    }


    public void OnToggleStickeMode(bool toggle)
    {
        lineGenerator.SwitchToStickerMode(toggle);
    }

    internal void OnResetStickers()
    {
        canvasStickerManager.OnStickerButtonResetAction?.Invoke();
    }



    internal Sprite GetCurrentSticker()
    {
        return canvasStickerManager.GetCurrentSticker();
    }


    public void OnImageCapture()
    {
        canvasAudioManager.OnCaptureImage();
    }


    public void CanDraw(bool val) => lineGenerator.CanDraw(val);

}
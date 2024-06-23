using System;
using UnityEngine;

public class AkibukiManager : MonoBehaviour
{
    public LineGenerator lineGenerator;
    public CanvasStickerManager canvasStickerManager;

    public Transform Holder;

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
}
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AkibukiManager : MonoBehaviour
{
    public LineGenerator lineGenerator;
    public CanvasStickerManager canvasStickerManager;
    public CanvasAudioManager canvasAudioManager;

    public Transform Holder;

    public AkibukiConfigSO akibukiConfigSO;
    public Image stickerImage;
    public RectTransform stickerRect;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        CanDraw(true);
        stickerImage.gameObject.SetActive(!akibukiConfigSO.isOpenCanbas);
        if (!akibukiConfigSO.isOpenCanbas)
        {

            stickerImage.sprite = akibukiConfigSO.sprite;
            stickerImage.SetNativeSize();
            // Calculate and set new size based on the desired height
            AdjustImageSize(stickerImage.sprite, 720);
        }
    }

    void AdjustImageSize(Sprite sprite, float fixedHeight)
    {
        if (sprite == null)
        {
            Debug.LogWarning("Sprite is null");
            return;
        }

        float aspectRatio = (float)sprite.texture.width / sprite.texture.height;
        float newWidth = fixedHeight * aspectRatio;

        // Set the size of the RectTransform
        stickerRect.sizeDelta = new Vector2(newWidth, fixedHeight);

        // Optionally, log the new size for debugging purposes
        Debug.Log($"Adjusted image size to: {newWidth}x{fixedHeight}");
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
    public void OnGoToHome() => SceneManager.LoadSceneAsync(1);

}
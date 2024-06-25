using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasStickerManager : MonoBehaviour
{

    public AkibukiManager akibukiManager;
    public Image stickerButton;
    public Transform strickerContainerParent;
    public GameObject stickerContainerPrefab;
    public RectTransform stickerSelectorBtn;

    public GameObject stickerPrefab;

    public SpritesContainerSO spritesContainerSO;

    public int stickerContainerIndex = 0;
    public int stickerIndex = 0;


    public Action<int> OnClickStickerButtonAction;
    public Action OnStickerButtonResetAction;
    public List<StickerButton> buttons;
    public List<GameObject> buttonsObjects;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        buttons = new();
        buttonsObjects = new();
        SpawnStickersOnIndex(0);

        for (int i = 0; i < spritesContainerSO.stickerContainers.Count; i++)
        {
            var obj = Instantiate(stickerContainerPrefab, strickerContainerParent);
            if (obj.TryGetComponent(out StickerContainerButton stickerContainerButton))
            {
                stickerContainerButton.id = i;
                stickerContainerButton.canvasStickerManager = this;
                stickerContainerButton.content.sprite = spritesContainerSO.stickerContainers[i].sprite;
                stickerContainerButton.button.onClick.AddListener(() =>
                {
                    strickerContainerParent.parent.transform.gameObject.SetActive(false);
                    akibukiManager.CanDraw(true);
                });
            }
        }
        stickerSelectorBtn.anchoredPosition = Vector3.zero;
        stickerSelectorBtn.sizeDelta = new(150, 150);
    }

    private void SpawnStickersOnIndex(int index)
    {
        stickerButton.sprite = spritesContainerSO.stickerContainers[index].sprite;
        if (buttons.Count > 0)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                OnClickStickerButtonAction -= buttons[i].OnUpdateButton;
                OnStickerButtonResetAction -= buttons[i].OnResetButton;
            }

            for (int i = buttons.Count - 1; i >= 0; i--)
            {
                Destroy(buttonsObjects[i]);
            }
            buttons.Clear();
            buttonsObjects.Clear();
        }


        var stickers = spritesContainerSO.stickerContainers[index];
        for (int i = 0; i < stickers.stickers.Count; i++)
        {
            var obj = Instantiate(stickerPrefab, this.transform);
            buttonsObjects.Add(obj);
            if (obj.TryGetComponent(out StickerButton button))
            {
                button.id = i;
                button.content.sprite = stickers.stickers[i];
                button.canvasStickerManager = this;
                OnClickStickerButtonAction += button.OnUpdateButton;
                OnStickerButtonResetAction += button.OnResetButton;
                buttons.Add(button);
            }
        }
    }

    public void SetSpriteContainerIndex(int val)
    {
        stickerContainerIndex = val;
        SpawnStickersOnIndex(val);
    }

    public void SetStickerIndex(int val)
    {
        stickerIndex = val;
        OnClickStickerButtonAction?.Invoke(val);
        akibukiManager.OnToggleStickeMode(true);
    }

    internal Sprite GetCurrentSticker()
    {
        return spritesContainerSO.stickerContainers[stickerContainerIndex].stickers[stickerIndex];
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DragAndDropManager : DropManager
{


    // Reference to the center drop container
    public DropContainer centerContainer;

    // List of all draggable objects
    public List<DraggableObject> Draggables = new();

    // Total number of objects placed
    public int TotalObjectsPlaced { get; private set; }

    public DragAndDropAudioPlayer dragAndDropAudioPlayer;


    // Reference to the PanelDataSO scriptable object
    public PanelDataSO panelDataSO;

    public UIManager uIManager;

    public TMP_FontAsset englishFont;
    public TMP_FontAsset arabicFont;
    public TMP_FontAsset banglaFont;

    private int totalAudio;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Construct the file path for the sorted image and audio files
        string dragCombFilePath = Application.persistentDataPath + $"/googli/{panelDataSO.gamePanelData.gameTypeName}/drag_comb/drag_comb";
        string dragFilePath = Application.persistentDataPath + $"/googli/{panelDataSO.gamePanelData.gameTypeName}/{panelDataSO.contentTypeFolderName}/{panelDataSO.contentTypeFolderName}";

        string prefix = "";
        var audioCombList = FileProcessor.GetSortedAudioFiles(dragCombFilePath, "aud_comb_");
        var combContList = FileProcessor.GetSortedImageFiles(dragCombFilePath, "comb_cont_");
        var combItemList = FileProcessor.GetSortedImageFiles(dragCombFilePath, "comb_itm_");
        var combIMList = FileProcessor.GetSortedImageFiles(dragCombFilePath, "im_comb_");


        if (FileProcessor.DNDTypes.TryGetValue(panelDataSO.gameName, out string val))
        {
            prefix = val;
        }

        // Get the sorted image and audio files from the file processor
        var audioList = FileProcessor.GetSortedAudioFiles(dragFilePath, prefix);


        MyDebug.Log($"Audio fOr prefix: {audioList.Count}");

        foreach (var al in audioList)
        {
            MyDebug.Log($"Audio : {al}");
        }

        totalAudio = audioList.Count;

        // Get the current level from the player preferences
        level = PlayerPrefs.GetInt($"{panelDataSO.gameName}", 0);

        // Play the first audio clip for the current level
        // FileProcessor.GetAudioClipByFileName(audioList[level], dragAndDropAudioPlayer.PlayFirstAudioClip);
        // Get the image file names for the right and left containers


        centerContainer.dropSide = DropSide.center;
        Sprite combContentSprite = centerContainer.currentRenderObject.sprite = FileProcessor.GetSpriteByFileName(combContList[level]);
        Sprite combItemSprite = centerContainer.currentRenderObject.sprite = FileProcessor.GetSpriteByFileName(combItemList[level]);
        Sprite combIMSprite = centerContainer.currentRenderObject.sprite = FileProcessor.GetSpriteByFileName(combIMList[level]);

        List<string> alphabetList = new();
        if (AlphabetLists.AlphabetTypes.TryGetValue(panelDataSO.gameName, out List<string> strList))
        {
            alphabetList = strList;
        }


        Debug.Log($"Alphabets: {alphabetList}");





        centerContainer.currentRenderObject.sprite = combContentSprite;
        centerContainer.CombinedRenderObject.sprite = combIMSprite;

        // Find all DraggableObject components in the scene and add them to the Draggables list
        Draggables = FindObjectsByType<DraggableObject>(FindObjectsSortMode.None).ToList();

        List<string> storedAlphabets = new();
        storedAlphabets.Add(alphabetList[level]);

        Draggables[0].spriteRenderer.sprite = combItemSprite;
        Draggables[0].dropSide = DropSide.center;
        Draggables[0].LetterText.text = storedAlphabets[0];
        Draggables[0].LetterText.font = SeTFont();


        for (int i = 1; i < Draggables.Count; i++)
        {
            Draggables[i].spriteRenderer.sprite = combItemSprite;
            Draggables[i].dropSide = DropSide.noSide;
            Draggables[i].LetterText.text = GetRandomAlphabet(alphabetList, storedAlphabets);
            Draggables[i].LetterText.font = SeTFont();
        }

        Utility.Shuffle(Draggables);


    }



    private TMP_FontAsset SeTFont()
    {
        if (AlphabetLists.AlphabetFontTypes.TryGetValue(panelDataSO.gameName, out string fonttype))
        {
            if (fonttype == "bangla")
            {
                return banglaFont;
            }
            else if (fonttype == "arabic")
            {
                return arabicFont;
            }
        }
        return englishFont;
    }

    private string GetRandomAlphabet(List<string> alphabetList, List<string> storedAlphabets)
    {
        var availableAlphabets = alphabetList.Except(storedAlphabets).ToList();
        if (availableAlphabets.Count == 0)
        {
            MyDebug.LogError("No more unique alphabets available.");
        }

        var random = new System.Random();
        var randomAlphabet = availableAlphabets[random.Next(availableAlphabets.Count)];
        storedAlphabets.Add(randomAlphabet);
        return randomAlphabet;
    }



    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        if (dragAndDropAudioPlayer.dragAndDropAudioPlayer.Count > 0)
        {
            foreach (var FlashCardAudioModel in dragAndDropAudioPlayer.dragAndDropAudioPlayer)
            {
                FlashCardAudioModel.audioIndex = 0;
            }
        }
        dragAndDropAudioPlayer.PlayAudioClipsSequentially(0);
    }



    // Method called when an object is dropped
    public override void OnDropObject(DraggableObject draggableObject, bool matched)
    {
        // If the object was not matched, return it to its original position
        if (!matched)
        {

            draggableObject.ReturnToOriginalPosition();
            return;
        }



        // Increment the total number of objects placed
        TotalObjectsPlaced++;

        draggableObject.OnSetSiteBoolEvent?.Invoke(false);

        // If all objects have been placed, trigger the game win logic
        //Todo:win
        Debug.LogWarning("Won");
        dragAndDropAudioPlayer.PlaySuccess(0);

        draggableObject.gameObject.SetActive(false);

        centerContainer.ActivateWinScenatio();
        timer.Stop();
    }


    public override void PlayFailedAudio()
    {
        dragAndDropAudioPlayer.PlayFailed(0);
    }

    public override void OnCancelDropObject(DraggableObject draggableObject)
    {
        // Decrement the total number of objects placed
        draggableObject.OnSetSiteBoolEvent?.Invoke(true);
        draggableObject.OnSetSiteTargetVec3Event?.Invoke(Vector3.zero);
    }
}
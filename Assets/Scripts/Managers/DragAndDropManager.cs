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



    private string initendAudioFile = "in_end.mp3";

    private List<string> initAudioFiles = new(){
        "in1.mp3",
        "in2.mp3",
        "in3.mp3"
    };

    private List<string> successAudioFiles = new(){
        "sc1.mp3",
        "sc2.mp3",
        "sc3.mp3"
    };

    private List<string> failedAudioFiles = new(){
        "fa1.mp3",
        "fa2.mp3",
        "fa3.mp3"
    };


    private int TOTAL_FILES_TO_LOAD = 0;
    private int TOTAL_FILES_LOADED = 0;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Construct the file path for the sorted image and audio files
        string dragCombFilePath = Application.persistentDataPath + $"/googli/{panelDataSO.gamePanelData.gameTypeName}/drag_comb/drag_comb";
        string dragFilePath = Application.persistentDataPath + $"/googli/{panelDataSO.gamePanelData.gameTypeName}/{panelDataSO.contentTypeFolderName}/{panelDataSO.contentTypeFolderName}";

        string prefix = "";

        // all combination audios
        var audioCombList = FileProcessor.GetSortedAudioFiles(dragCombFilePath, "aud_comb_");

        // getting all audios
        var allAudios = FileProcessor.GetSortedAudioFiles(dragCombFilePath);
        // getting all combined content
        var combContList = FileProcessor.GetSortedImageFiles(dragCombFilePath, "comb_cont_");
        //getting all combined item
        var combItemList = FileProcessor.GetSortedImageFiles(dragCombFilePath, "comb_itm_");
        //getting all final combinations
        var combIMList = FileProcessor.GetSortedImageFiles(dragCombFilePath, "im_comb_");


        // Get the current level from the player preferences
        level = PlayerPrefs.GetInt($"{panelDataSO.gameName}", 0);


        TOTAL_FILES_TO_LOAD++;
        FileProcessor.GetAudioClipByFileName($"{dragCombFilePath}\\{initendAudioFile}", (clip) =>
        {
            dragAndDropAudioPlayer.OnsetInitEndAudio(clip);
            LoadCheck();
        });


        var initPaths = FileProcessor.FetchFilePaths(allAudios, initAudioFiles);


        foreach (var initPath in initPaths)
        {
            MyDebug.Log($"Init Path Audio: {initPath}");
            TOTAL_FILES_TO_LOAD++;
            FileProcessor.GetAudioClipByFileName(initPath, (clip) =>
        {
            dragAndDropAudioPlayer.OnsetInitAudio(clip);
            LoadCheck();
        });
        }

        var failsPaths = FileProcessor.FetchFilePaths(allAudios, failedAudioFiles);
        foreach (var failsPath in failsPaths)
        {
            TOTAL_FILES_TO_LOAD++;
            FileProcessor.GetAudioClipByFileName(failsPath, (clip) =>
        {
            dragAndDropAudioPlayer.OnsetFailedAudio(clip);
            LoadCheck();
        });
        }


        var successPaths = FileProcessor.FetchFilePaths(allAudios, successAudioFiles);
        foreach (var successPath in successPaths)
        {
            TOTAL_FILES_TO_LOAD++;
            FileProcessor.GetAudioClipByFileName(successPath, (clip) =>
        {
            dragAndDropAudioPlayer.OnsetSuccessAudio(clip);
            LoadCheck();
        });
        }


        foreach (var ac in audioCombList)
        {
            MyDebug.Log($"Comb: {ac}");
        }

        TOTAL_FILES_TO_LOAD++;
        FileProcessor.GetAudioClipByFileName(audioCombList[level], (clip) =>
        {
            dragAndDropAudioPlayer.OnsetCombinedAudio(clip);
            LoadCheck();
        });
        // getting prefix
        if (FileProcessor.DNDTypes.TryGetValue(panelDataSO.gameName, out string val))
        {
            prefix = val;
        }

        // Get the sorted image and audio files from the file processor
        var audioList = FileProcessor.GetSortedAudioFiles(dragFilePath, prefix);


        MyDebug.Log($"Audio fOr prefix: {allAudios.Count}");

        foreach (var al in audioList)
        {
            MyDebug.Log($"Audio : {al}");
        }

        totalAudio = audioList.Count;


        // Play the first audio clip for the current level
        // FileProcessor.GetAudioClipByFileName(audioList[level], dragAndDropAudioPlayer.PlayFirstAudioClip);
        // Get the image file names for the right and left containers


        centerContainer.dropSide = DropSide.center;
        Sprite combContentSprite = centerContainer.currentRenderObject.sprite = FileProcessor.GetSpriteByFileName(combContList[level], 2);
        Sprite combItemSprite = centerContainer.currentRenderObject.sprite = FileProcessor.GetSpriteByFileName(combItemList[level], 3);
        Sprite combIMSprite = centerContainer.currentRenderObject.sprite = FileProcessor.GetSpriteByFileName(combIMList[level], 2);
        TOTAL_FILES_TO_LOAD++;
        FileProcessor.GetAudioClipByFileName(audioList[level], (clip) =>
        {
            dragAndDropAudioPlayer.OnsetInitLetterAudio(clip);
            LoadCheck();
        });

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

        List<string> storedAlphabets = new()
        {
            alphabetList[level]
        };

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


    public void LoadCheck()
    {
        TOTAL_FILES_LOADED++;
        if (TOTAL_FILES_TO_LOAD == TOTAL_FILES_LOADED)
        {
            MyDebug.Log("Playing");
            StarCounts = 3;
            dragAndDropAudioPlayer.SetSequence();
            dragAndDropAudioPlayer.PlayAudioClipsSequentially(0);
        }
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
    void Start()
    {
        // if (dragAndDropAudioPlayer.dragAndDropAudioPlayer.Count > 0)
        // {
        //     foreach (var FlashCardAudioModel in dragAndDropAudioPlayer.dragAndDropAudioPlayer)
        //     {
        //         FlashCardAudioModel.audioIndex = 0;
        //     }
        // }
        // dragAndDropAudioPlayer.PlayAudioClipsSequentially(0);
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
        dragAndDropAudioPlayer.PlaySuccess();
        uIManager.OnShowGameOverPanel();
        draggableObject.gameObject.SetActive(false);

        centerContainer.ActivateWinScenatio();
        timer.Stop();
    }


    public override void PlayFailedAudio()
    {
        dragAndDropAudioPlayer.PlayFailed();
    }

    public override void OnCancelDropObject(DraggableObject draggableObject)
    {
        // Decrement the total number of objects placed
        draggableObject.OnSetSiteBoolEvent?.Invoke(true);
        draggableObject.OnSetSiteTargetVec3Event?.Invoke(Vector3.zero);
    }

    public override void SaveLevel()
    {
        MyDebug.Log("SAVING.......");
        PlayerPrefs.SetInt($"{panelDataSO.gameName}", (level == totalAudio - 1) ? 0 : ++level);
        MyDebug.Log("SAVING......." + level);
        PlayerPrefs.Save();
    }
}
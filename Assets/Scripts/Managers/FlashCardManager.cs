using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class FlashCardManager : LevelBaseManager
{

    public List<FlashCardHolder> flashCards = new();

    public FlashCardAudioPlayer flashCardAudioPlayer;

    public GameObject flashPrefab;


    // Reference to the PanelDataSO scriptable object
    public PanelDataSO panelDataSO;


    public int currentCardIndex;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Construct the file path for the sorted image and audio files

        string flashCardFilePath = Application.persistentDataPath + $"/googli/{panelDataSO.gamePanelData.gameTypeName}/{panelDataSO.contentTypeFolderName}/{panelDataSO.contentTypeFolderName}";

        string prefix = (panelDataSO.gameName == "shoroborno") ? "BSB" : (panelDataSO.gameName == "banjonborno") ? "BNG" : "C_";

        List<string> allImages = FileProcessor.GetSortedImageFiles(flashCardFilePath);
        List<string> flashCardAlphabetsimages = new();


        List<string> flashCardCharacterimages = new();



        List<string> filteredCharactersimages = new();

        if (panelDataSO.gameName != "english_alphabets")
        {

            flashCardAlphabetsimages = allImages
            .Where(f => !Path.GetFileName(f).Contains('_')).OrderBy(s => ExtractNumber(s))
            .ToList();


            filteredCharactersimages = allImages.Where(f => Path.GetFileNameWithoutExtension(f).EndsWith((panelDataSO.gameName == "shoroborno") ? "_W" : "_WM")).OrderBy(s => ExtractNumber(Path.GetFileNameWithoutExtension(s)))
            .ToList();

        }

        else
        {
            flashCardAlphabetsimages = FileProcessor.GetSortedImageFiles(flashCardFilePath, prefix);
            flashCardCharacterimages = FileProcessor.GetSortedImageFiles(flashCardFilePath);

            // Filter and sort the strings
            filteredCharactersimages = flashCardCharacterimages
               .Where(s => !Path.GetFileName(s).StartsWith(prefix)) // Use Path.GetFileName to filter by file name only
               .OrderBy(s => Path.GetFileName(s))                  // Sort by the file name
               .ToList();
        }




        // all combination audios
        var audioList = FileProcessor.GetSortedAudioFiles(flashCardFilePath);
        List<string> letterAudioList = new();
        List<string> wordAudioList = new();
        List<string> sentenceAudioList = new();

        if (panelDataSO.gameName != "english_alphabets")
        {
            letterAudioList = audioList.Where(s => !Path.GetFileName(s).Contains('_')).OrderBy(s => ExtractNumber(Path.GetFileNameWithoutExtension(s))).ToList();
            for (int i = 0; i < wordAudioList.Count; i++)
            {
                MyDebug.Log($"wordAudioList Name: {wordAudioList[i]}");
            }

            wordAudioList = audioList.Where(f => Path.GetFileNameWithoutExtension(f).EndsWith("_W")).OrderBy(s => ExtractNumber(Path.GetFileNameWithoutExtension(s)))
         .ToList();

            for (int i = 0; i < wordAudioList.Count; i++)
            {
                MyDebug.Log($"wordAudioList Name: {wordAudioList[i]}");
            }
            sentenceAudioList = audioList.Where(f => Path.GetFileNameWithoutExtension(f).EndsWith("_WM")).OrderBy(s => ExtractNumber(Path.GetFileNameWithoutExtension(s)))
            .ToList();

            for (int i = 0; i < sentenceAudioList.Count; i++)
            {
                MyDebug.Log($"sentenceAudioList Name: {sentenceAudioList[i]}");
            }

        }

        else
        {
            letterAudioList = audioList.Where(s => Path.GetFileNameWithoutExtension(s).Length == 1).ToList();
            // Remove these filtered files from the original list
            audioList = audioList
                .Where(s => !letterAudioList.Contains(s))
                .ToList();
        }

        foreach (var word in letterAudioList)
        {
            if (panelDataSO.gameName == "shoroborno")
            {
                // // Custom sorting logic
                // var tags = audioList
                //     .OrderBy(s => GetBasePart(s))    // First, sort by the numeric part
                //     .ThenBy(s => GetSuffixPart(s))   // Then, sort by the suffix part
                //     .ToList();


                // MyDebug.Log($"Tag Audio Name 1: {tags[0]}");
                // MyDebug.Log($"Tag Audio Name 2: {tags[1]}");

                // wordAudioList.Add(tags[0]);
                // sentenceAudioList.Add(tags[1]);
            }
            else
            {
                var _word = Path.GetFileName(word).Split('.')[0];
                MyDebug.Log($"Word Path Name: {_word}");
                var tags = audioList.Where(s => Path.GetFileName(s).StartsWith(_word))
                            .OrderBy(s => Path.GetFileName(s))                  // Sort by the file name
                .ToList();

                MyDebug.Log($"Tag Audio Name 1: {tags[0]}");
                MyDebug.Log($"Tag Audio Name 2: {tags[1]}");

                wordAudioList.Add(tags[0]);
                sentenceAudioList.Add(tags[1]);
            }
        }

        // for (int i = 0; i < wordAudioList.Count; i++)
        // {
        //     MyDebug.Log($"wordAudioList Name: {wordAudioList[i]}");
        // }

        // for (int i = 0; i < sentenceAudioList.Count; i++)
        // {
        //     MyDebug.Log($"sentenceAudioList Name: {sentenceAudioList[i]}");
        // }


        flashCardAudioPlayer.Initialize(letterAudioList.Count);

        // Get the current level from the player preferences
        level = PlayerPrefs.GetInt($"{panelDataSO.gameName}", 0);
        currentCardIndex = level;
        for (int i = 0; i < letterAudioList.Count; i++)
        {
            flashCards.Add(new());
        }
        var obj = Instantiate(flashPrefab, transform);
        if (obj.TryGetComponent(out FlashCardHolder component))
        {
            flashCards[level] = component;
            component.OnFlashInitailizedCompleteEvnet.AddListener(() => OnFlashInitailizedComplete());
            component.OnFlashClosedCompleteEvent.AddListener(() => OnFlashClosedComplete());
            component.OnFlashOpenedCompleteEvent.AddListener(() => OnFlashOpenedComplete());

        }

        MyDebug.Log($"{flashCardAlphabetsimages[level]}");
        MyDebug.Log($"{filteredCharactersimages[level]}");
        component.LetterImage.sprite = FileProcessor.GetSpriteByFileName($"{flashCardAlphabetsimages[level]}", targetCells: 4);
        component.CharacterImage.sprite = FileProcessor.GetSpriteByFileName($"{filteredCharactersimages[level]}", targetCells: 6);
        if (FlashCardsNames.CardNames.TryGetValue(panelDataSO.gameName, out List<string> strs))
        {
            component.Word.text = strs[level];
        }


        FileProcessor.GetAudioClipByFileName($"{letterAudioList[level]}", (clip) =>
                       {
                           flashCardAudioPlayer.OnAudioStartByLetter(clip, level);

                       });
        FileProcessor.GetAudioClipByFileName($"{wordAudioList[level]}", (clip) =>
        {
            flashCardAudioPlayer.OnAudioStartByWord(clip, level);

        }); FileProcessor.GetAudioClipByFileName($"{sentenceAudioList[level]}", (clip) =>
        {
            flashCardAudioPlayer.OnAudioStartBySentence(clip, level);

        });

        for (int i = 0; i < letterAudioList.Count; i++)

        {
            if (i == level) continue;

            AddAudioForIndex(letterAudioList, wordAudioList, sentenceAudioList, flashCardAlphabetsimages, filteredCharactersimages, i);
        }

        StarCounts = 3;


    }

    private void AddAudioForIndex(List<string> letterAudioList, List<string> wordAudioList, List<string> sentenceAudioList, List<string> flashCardAlphabetsimages, List<string> filteredCharactersimages, int i)
    {

        var obj = Instantiate(flashPrefab, transform);
        obj.SetActive(false);
        if (obj.TryGetComponent(out FlashCardHolder component))
        {
            flashCards[i] = component;
            component.OnFlashInitailizedCompleteEvnet.AddListener(() => OnFlashInitailizedComplete());
            component.OnFlashClosedCompleteEvent.AddListener(() => OnFlashClosedComplete());
            component.OnFlashOpenedCompleteEvent.AddListener(() => OnFlashOpenedComplete());
        }
        component.LetterImage.sprite = FileProcessor.GetSpriteByFileName($"{flashCardAlphabetsimages[i]}", targetCells: 4);
        component.CharacterImage.sprite = FileProcessor.GetSpriteByFileName($"{filteredCharactersimages[i]}", targetCells: 6);

        if (FlashCardsNames.CardNames.TryGetValue(panelDataSO.gameName, out List<string> strs))
        {
            component.Word.text = strs[i];
        }

        FileProcessor.GetAudioClipByFileName($"{letterAudioList[i]}", (clip) =>
       {
           flashCardAudioPlayer.OnAudioStartByLetter(clip, i);
       });
        FileProcessor.GetAudioClipByFileName($"{wordAudioList[i]}", (clip) =>
        {
            flashCardAudioPlayer.OnAudioStartByWord(clip, i);

        }); FileProcessor.GetAudioClipByFileName($"{sentenceAudioList[i]}", (clip) =>
        {
            flashCardAudioPlayer.OnAudioStartBySentence(clip, i);

        });
    }



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        flashCards[currentCardIndex].OpenCard();
    }

    public void OnFlashInitailizedComplete()
    {
        //Todo: Play Audio
        flashCardAudioPlayer.PlayFirstAudioClip(currentCardIndex);
    }

    public void OnFlashClosedComplete()
    {

    }


    public void OnFlashOpenedComplete()
    {
        //Todo: Play Audio

        flashCardAudioPlayer.PlayAudioClipsSequentially(currentCardIndex);
    }

    internal void GoToNextCard()
    {
        flashCardAudioPlayer.ResetAudioIndexForcard(currentCardIndex);
        flashCards[currentCardIndex].gameObject.SetActive(false);
        currentCardIndex++;
        level = currentCardIndex;
        SaveLevel();
        flashCards[currentCardIndex].gameObject.SetActive(true);
        flashCards[currentCardIndex].OpenCard();
    }

    internal void GoToPreviousCard()
    {
        flashCardAudioPlayer.ResetAudioIndexForcard(currentCardIndex);
        flashCards[currentCardIndex].gameObject.SetActive(false);
        currentCardIndex--;
        level = currentCardIndex;
        SaveLevel();
        flashCards[currentCardIndex].gameObject.SetActive(true);

        flashCards[currentCardIndex].OpenCard();
    }

    private static int ExtractNumber(string filePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string numericPart = new string(fileName.SkipWhile(c => !char.IsDigit(c)).TakeWhile(char.IsDigit).ToArray());
        return int.TryParse(numericPart, out int number) ? number : int.MaxValue;
    }

    public override void SaveLevel()
    {
        MyDebug.Log("SAVING.......");
        PlayerPrefs.SetInt($"{panelDataSO.gameName}", level);
        MyDebug.Log("SAVING......." + level);
        PlayerPrefs.Save();
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FlashCardManager : LevelBaseManager
{
    public List<FlashCardHolder> flashCards = new();
    public List<Sprite> flashcardSprites = new();
    public List<Sprite> backGroundSprites = new();
    public Image background;
    public FlashCardAudioPlayer flashCardAudioPlayer;
    public GameObject flashPrefab;

    // Reference to the PanelDataSO scriptable object
    public PanelDataSO panelDataSO;
    public int currentCardIndex;

    void Awake()
    {
        // Construct the file path for the sorted image and audio files
        string flashCardFilePath = Application.persistentDataPath + $"/googli/{panelDataSO.gamePanelData.gameTypeName}/{panelDataSO.contentTypeFolderName}/{panelDataSO.contentTypeFolderName}";

        string prefix = (panelDataSO.gameName == "shoroborno") ? "BSB" : (panelDataSO.gameName == "banjonborno") ? "BNG" : "C_";
        Sprite backCardImage = (panelDataSO.gameName == "banjonborno") ? flashcardSprites[0] : (panelDataSO.gameName == "shoroborno") ? flashcardSprites[2] : flashcardSprites[4];
        Sprite frontCardImage = (panelDataSO.gameName == "banjonborno") ? flashcardSprites[1] : (panelDataSO.gameName == "shoroborno") ? flashcardSprites[3] : flashcardSprites[5];
        background.sprite = (panelDataSO.gameName == "banjonborno") ? backGroundSprites[0] : (panelDataSO.gameName == "shoroborno") ? backGroundSprites[1] : backGroundSprites[2];

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
            filteredCharactersimages = flashCardCharacterimages
                .Where(s => !Path.GetFileName(s).StartsWith(prefix))
                .OrderBy(s => Path.GetFileName(s))
                .ToList();
        }

        // All combination audios
        var audioList = FileProcessor.GetSortedAudioFiles(flashCardFilePath);
        List<string> letterAudioList = new();
        List<string> wordAudioList = new();
        List<string> sentenceAudioList = new();

        if (panelDataSO.gameName != "english_alphabets")
        {
            letterAudioList = audioList.Where(s => !Path.GetFileName(s).Contains('_')).OrderBy(s => ExtractNumber(Path.GetFileNameWithoutExtension(s))).ToList();
            wordAudioList = audioList.Where(f => Path.GetFileNameWithoutExtension(f).EndsWith("_W")).OrderBy(s => ExtractNumber(Path.GetFileNameWithoutExtension(s))).ToList();
            sentenceAudioList = audioList.Where(f => Path.GetFileNameWithoutExtension(f).EndsWith("_WM")).OrderBy(s => ExtractNumber(Path.GetFileNameWithoutExtension(s))).ToList();
        }
        else
        {
            letterAudioList = audioList.Where(s => Path.GetFileNameWithoutExtension(s).Length == 1).ToList();
            audioList = audioList.Where(s => !letterAudioList.Contains(s)).ToList();
        }

        foreach (var word in letterAudioList)
        {
            if (panelDataSO.gameName == "shoroborno")
            {
                // Handle special case for shoroborno
            }
            else
            {
                var _word = Path.GetFileName(word).Split('.')[0];
                var tags = audioList.Where(s => Path.GetFileName(s).StartsWith(_word))
                            .OrderBy(s => Path.GetFileName(s))
                            .ToList();

                if (tags.Count >= 2)
                {
                    wordAudioList.Add(tags[0]);
                    sentenceAudioList.Add(tags[1]);
                }
            }
        }

        flashCardAudioPlayer.Initialize(letterAudioList.Count);

        // Get the current level from the player preferences
        level = MyPlayerPrefabs.Instance.GetInt($"{panelDataSO.gameName}", 0);
        currentCardIndex = level;
        for (int i = 0; i < letterAudioList.Count; i++)
        {
            flashCards.Add(null);  // Initialize the list with nulls
        }

        InstantiateFlashCard(level, frontCardImage, backCardImage, flashCardAlphabetsimages, filteredCharactersimages, letterAudioList, wordAudioList, sentenceAudioList);

        for (int i = 0; i < letterAudioList.Count; i++)
        {
            if (i == level) continue;
            AddAudioForIndex(frontCardImage, backCardImage, letterAudioList, wordAudioList, sentenceAudioList, flashCardAlphabetsimages, filteredCharactersimages, i);
        }

        StarCounts = 3;
    }

    private void InstantiateFlashCard(int index, Sprite frontCardImage, Sprite backCardImage, List<string> flashCardAlphabetsimages, List<string> filteredCharactersimages, List<string> letterAudioList, List<string> wordAudioList, List<string> sentenceAudioList)
    {
        var obj = Instantiate(flashPrefab, transform);

        if (obj.TryGetComponent(out FlashCardHolder component))
        {
            component.flashCardFront.frontFace = frontCardImage;
            component.flashCardBack.backFace = backCardImage;
            flashCards[index] = component;
            component.InitailizedCard();
            component.OnFlashInitailizedCompleteEvnet.AddListener(() => OnFlashInitailizedComplete());
            component.OnFlashClosedCompleteEvent.AddListener(() => OnFlashClosedComplete());
            component.OnFlashOpenedCompleteEvent.AddListener(() => OnFlashOpenedComplete());

            component.LetterImage.sprite = FileProcessor.GetSpriteByFileName($"{flashCardAlphabetsimages[index]}", targetCells: 4);
            component.CharacterImage.sprite = FileProcessor.GetSpriteByFileName($"{filteredCharactersimages[index]}", targetCells: 6);

            FileProcessor.GetAudioClipByFileName($"{letterAudioList[index]}", (clip) =>
            {
                flashCardAudioPlayer.OnAudioStartByLetter(clip, index);
            });

            FileProcessor.GetAudioClipByFileName($"{wordAudioList[index]}", (clip) =>
            {
                flashCardAudioPlayer.OnAudioStartByWord(clip, index);
            });

            FileProcessor.GetAudioClipByFileName($"{sentenceAudioList[index]}", (clip) =>
            {
                flashCardAudioPlayer.OnAudioStartBySentence(clip, index);
            });
        }
    }

    private void AddAudioForIndex(Sprite frontCardImage, Sprite backCardImage, List<string> letterAudioList, List<string> wordAudioList, List<string> sentenceAudioList, List<string> flashCardAlphabetsimages, List<string> filteredCharactersimages, int i)
    {
        var obj = Instantiate(flashPrefab, transform);
        obj.SetActive(false);
        if (obj.TryGetComponent(out FlashCardHolder component))
        {
            component.flashCardFront.frontFace = frontCardImage;
            component.flashCardBack.backFace = backCardImage;
            flashCards[i] = component;
            component.InitailizedCard();
            component.OnFlashInitailizedCompleteEvnet.AddListener(() => OnFlashInitailizedComplete());
            component.OnFlashClosedCompleteEvent.AddListener(() => OnFlashClosedComplete());
            component.OnFlashOpenedCompleteEvent.AddListener(() => OnFlashOpenedComplete());

            component.LetterImage.sprite = FileProcessor.GetSpriteByFileName($"{flashCardAlphabetsimages[i]}", targetCells: 4);
            component.CharacterImage.sprite = FileProcessor.GetSpriteByFileName($"{filteredCharactersimages[i]}", targetCells: 6);

            FileProcessor.GetAudioClipByFileName($"{letterAudioList[i]}", (clip) =>
            {
                flashCardAudioPlayer.OnAudioStartByLetter(clip, i);
            });

            FileProcessor.GetAudioClipByFileName($"{wordAudioList[i]}", (clip) =>
            {
                flashCardAudioPlayer.OnAudioStartByWord(clip, i);
            });

            FileProcessor.GetAudioClipByFileName($"{sentenceAudioList[i]}", (clip) =>
            {
                flashCardAudioPlayer.OnAudioStartBySentence(clip, i);
            });
        }
    }

    void Start()
    {
        flashCards[currentCardIndex].gameObject.SetActive(true);  // Ensure the current card is active
        flashCards[currentCardIndex].OpenCard();
    }

    public void OnFlashInitailizedComplete()
    {
        flashCardAudioPlayer.PlayFirstAudioClip(currentCardIndex);
    }

    public void OnFlashClosedComplete() { }

    public void OnFlashOpenedComplete()
    {
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
        MyPlayerPrefabs.Instance.SetInt($"{panelDataSO.gameName}", level);
    }
}

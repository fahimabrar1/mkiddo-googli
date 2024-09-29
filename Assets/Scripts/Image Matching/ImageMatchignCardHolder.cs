using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageMatchignCardHolder : LevelBaseManager
{

    public List<ImageMatchingCard> cards;
    public List<ImageMatchingCard> cardsFormatch;

    private int MAX_LEVEL = 3;
    private int toBeMatch = 0;

    // Reference to the PanelDataSO scriptable object
    public ImageMatchingAudioPlayer imageMatchingAudioPlayer;
    public PanelDataSO panelDataSO;


    public UIManager uIManager;

    // Start is called before the first frame update
    void Awake()
    {

        // Construct the file path for the sorted image and audio files
        string filePath = Application.persistentDataPath + $"/googli/{panelDataSO.gamePanelData.gameTypeName}/{panelDataSO.contentTypeFolderName}/{panelDataSO.contentTypeFolderName}";

        // Get the sorted image and audio files from the file processor
        var imagesList = FileProcessor.GetSortedImageFiles(filePath);
        var audioList = FileProcessor.GetSortedAudioFiles(filePath);


        cardsFormatch = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.TryGetComponent(out ImageMatchingCard imageMatchingCard))
            {
                imageMatchingCard.CardID = i;
                imageMatchingCard.imageMatchignCardHolder = this;

                cards.Add(imageMatchingCard);
            }
        }
    }



    public void OnClickForMatch(ImageMatchingCard imageMatchingCard)
    {
        if (cardsFormatch.Count < 2)
            cardsFormatch.Add(imageMatchingCard);
        if (cardsFormatch.Count == 2)
        {
            if (cardsFormatch[0].CombinationID == cardsFormatch[1].CombinationID)
            {
                foreach (var card in cardsFormatch)
                {
                    card.FadeOutBottomToTop();
                }

                cardsFormatch.Clear();
                toBeMatch++;

                if (toBeMatch == 5)
                {
                    //Todo:GameOver
                }
            }
            else
            {
                // then it's  not a match
                foreach (var card in cardsFormatch)
                {
                    StartCoroutine(card.RotateCard180To90Deg());
                }
                cardsFormatch.Clear();
            }
        }
    }


    public override void SaveLevel()
    {
        MyDebug.Log("SAVING.......");
        MyPlayerPrefabs.Instance.SetInt($"{panelDataSO.gameName}", (level == MAX_LEVEL - 1) ? 0 : ++level);
    }
}

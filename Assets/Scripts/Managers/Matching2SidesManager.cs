using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Matching2SidesManager : LevelBaseManager
{
    public enum Matching2SidesManagerType
    {
        right, left
    }
    // Reference to the PanelDataSO scriptable object
    public PanelDataSO panelDataSO;

    // Reference to the ImageSortingAudioPLayer scriptable object
    public Matching2SidesAudioManager matching2SidesAudioManaher;

    public UIManager uIManager;

    public GameObject spriteLineRendererPrefab; // Prefab with SpriteLineRenderer component
    public MathingSideContainer leftContainer; // Reference to the left container
    public MathingSideContainer rightContainer; // Reference to the right container

    [SerializeField] private List<SpriteLineDrawer> spriteLineRenderers = new();

    private List<Sprite> leftSprites = new List<Sprite>();
    private List<Sprite> rightSprites = new List<Sprite>();

    public int MATCHES_TO_BE_MADE = 0;
    public int MATCHES_MADE = 0;
    public int TOTAL_PAIRS = 0;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Construct the file path for the sorted image and audio files
        string filePath = Application.persistentDataPath + $"/googli/{panelDataSO.gamePanelData.gameTypeName}/{panelDataSO.contentTypeFolderName}/{panelDataSO.contentTypeFolderName}";

        // Get the sorted image and audio files from the file processor
        var imagesList = FileProcessor.GetSortedImageFilesForMatchingSides(filePath);
        var audioList = FileProcessor.GetSortedAudioFiles(filePath);
        level = PlayerPrefs.GetInt($"{panelDataSO.gameName}", 0);

        // Process image pairs
        ProcessImagePairs(imagesList);

        foreach (var imagename in imagesList)
        {
            MyDebug.Log("Image Name: " + imagename);
        }
    }

    void Start()
    {
        // Ensure we have even sprites in both containers
        if (leftSprites.Count != rightSprites.Count)
        {
            Debug.LogError("Mismatch in the number of left and right sprites.");
            return;
        }

        // Assign sprites to containers
        leftContainer.SetSprites(leftSprites);
        rightContainer.SetSprites(rightSprites);

        // Create n SpriteLineRenderer instances for 2n sprites
        int n = leftSprites.Count;
        for (int i = 0; i < n; i++)
        {
            GameObject lineObj = Instantiate(spriteLineRendererPrefab);
            SpriteLineDrawer lineRenderer = lineObj.GetComponent<SpriteLineDrawer>();
            spriteLineRenderers.Add(lineRenderer);
        }
    }

    private void ProcessImagePairs(List<string> imagesList)
    {
        int pairsPerLevel = 4; // Number of pairs to show per level
        TOTAL_PAIRS = imagesList.Count / 2; // Total number of pairs available

        int startIndex = level * pairsPerLevel; // Calculate the starting index for the current level

        // Calculate the number of pairs to show for the current level
        int pairsToShow = Mathf.Min(pairsPerLevel, TOTAL_PAIRS - startIndex);

        // Loop through the required pairs and add them to the respective lists
        for (int i = startIndex * 2; i < (startIndex + pairsToShow) * 2; i += 2)
        {
            // Construct file paths for left and right images
            string leftImagePath = imagesList[i];
            string rightImagePath = imagesList[i + 1];

            // Load sprites and add them to the respective lists
            leftSprites.Add(LoadSprite(leftImagePath));
            rightSprites.Add(LoadSprite(rightImagePath));
            MATCHES_TO_BE_MADE++;
        }
    }

    private Sprite LoadSprite(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public SpriteLineDrawer GetAvailableLineRenderer()
    {
        foreach (var lineRenderer in spriteLineRenderers)
        {
            if (!lineRenderer.IsLineActive && lineRenderer.IsLineAvailavle)
            {
                return lineRenderer;
            }
        }
        return null;
    }

    internal void OnSideMatch()
    {
        MATCHES_MADE++;
        if (MATCHES_TO_BE_MADE == MATCHES_MADE)
        {
            uIManager.OnShowGameOverPanel();
        }
    }


    public override void SaveLevel()
    {
        int finalLevel = (TOTAL_PAIRS % 4 == 0) ? Mathf.FloorToInt(TOTAL_PAIRS / 4 - 1) : Mathf.FloorToInt(TOTAL_PAIRS / 4 - 1) + 1;
        PlayerPrefs.SetInt($"{panelDataSO.gameName}", (level == finalLevel) ? 0 : ++level);
        PlayerPrefs.Save();
    }
}
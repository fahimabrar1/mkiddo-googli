using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class ImageSortingManager : DropManager
{

    // Reference to the left drop container
    public DropContainer leftContainer;

    // Reference to the right drop container
    public DropContainer rightContainer;

    // List of all draggable objects
    public List<DraggableObject> Draggables = new();

    // Total number of objects placed
    public int TotalObjectsPlaced { get; private set; }

    // Reference to the PanelDataSO scriptable object
    public PanelDataSO panelDataSO;

    // Reference to the ImageSortingAudioPLayer scriptable object
    public ImageSortingAudioPLayer imageSortingAudioPLayer;

    public UIManager uIManager;

    private int totalAudio;

    private void Awake()
    {
        // Construct the file path for the sorted image and audio files
        string filePath = Application.persistentDataPath + $"/googli/{panelDataSO.gamePanelData.gameTypeName}/{panelDataSO.contentTypeFolderName}/{panelDataSO.contentTypeFolderName}";

        // Get the sorted image and audio files from the file processor
        var imagesList = FileProcessor.GetSortedImageFiles(filePath);
        var audioList = FileProcessor.GetSortedAudioFiles(filePath);

        totalAudio = audioList.Count;

        // Get the current level from the player preferences
        level = MyPlayerPrefabs.Instance.GetInt($"{panelDataSO.gameName}", 0);

        // Play the first audio clip for the current level
        FileProcessor.GetAudioClipByFileName(audioList[level], imageSortingAudioPLayer.PlayFirstAudioClip);

        // Get the image file names for the right and left containers
        string imageIndexRight = imagesList[level * 2];
        string imageIndexLeft = imagesList[(level * 2) + 1];

        // Set the drop side for the right and left containers
        rightContainer.dropSide = DropSide.right;
        leftContainer.dropSide = DropSide.left;

        // Set the sprite for the right and left containers
        Sprite rightSprite = rightContainer.currentRenderObject.sprite = FileProcessor.GetSpriteByFileName(imageIndexRight, 4);
        Sprite leftSprite = leftContainer.currentRenderObject.sprite = FileProcessor.GetSpriteByFileName(imageIndexLeft, 4);

        // Find all DraggableObject components in the scene and add them to the Draggables list
        Draggables = FindObjectsByType<DraggableObject>(FindObjectsSortMode.None).ToList();

        // Divide the Draggables list into two halves and assign the right and left sprites accordingly
        int halfCount = Draggables.Count / 2;

        for (int i = 0; i < halfCount; i++)
        {
            Draggables[i].spriteRenderer.sprite = rightSprite;
            Draggables[i].dropSide = DropSide.right;
        }

        for (int i = halfCount; i < Draggables.Count; i++)
        {
            Draggables[i].spriteRenderer.sprite = leftSprite;
            Draggables[i].dropSide = DropSide.left;
        }
        // Shuffle the list of Draggables
        Utility.Shuffle(Draggables);
        StarCounts = 3;
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
        if (TotalObjectsPlaced == Draggables.Count)
        {
            // Todo: Game Win
            uIManager.OnShowGameOverPanel();
            timer.Stop();
        }
    }




    public override void OnCancelDropObject(DraggableObject draggableObject)
    {
        // Decrement the total number of objects placed
        TotalObjectsPlaced--;
        draggableObject.OnSetSiteBoolEvent?.Invoke(true);
        draggableObject.OnSetSiteTargetVec3Event?.Invoke(Vector3.zero);
    }


    public override void SaveLevel()
    {
        MyPlayerPrefabs.Instance.SetInt($"{panelDataSO.gameName}", (level == totalAudio - 1) ? 0 : ++level);
    }
}
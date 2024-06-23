using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGallery : MonoBehaviour
{
    public GameObject imagePrefab; // Prefab with an Image component
    public Transform contentPanel; // Parent panel to hold the images

    public List<GameObject> images;
    private void OnEnable()
    {
        images = new();
        DisplaySavedImages();
    }


    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        // Clear existing images
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
        images.Clear();
    }

    private void DisplaySavedImages()
    {


        string[] imageFiles = Directory.GetFiles(Application.persistentDataPath, "SavedImage_*.png");

        foreach (string filePath in imageFiles)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);

            GameObject newImage = Instantiate(imagePrefab, contentPanel);
            images.Add(newImage);
            newImage.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
    }
}

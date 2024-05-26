using System;
using System.Collections;
using System.Collections.Generic;
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
    public List<SpriteRenderer> sprites; // List of all sprites

    [SerializeField] private List<SpriteLineDrawer> spriteLineRenderers = new();

    void Start()
    {
        // Ensure we have an even number of sprites
        if (sprites.Count % 2 != 0)
        {
            Debug.LogError("Number of sprites must be even.");
            return;
        }

        // Create n SpriteLineRenderer instances for 2n sprites
        int n = sprites.Count / 2;
        for (int i = 0; i < n; i++)
        {
            GameObject lineObj = Instantiate(spriteLineRendererPrefab);
            SpriteLineDrawer lineRenderer = lineObj.GetComponent<SpriteLineDrawer>();
            spriteLineRenderers.Add(lineRenderer);
        }
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

    // public override void SaveLevel()
    // {
    //     PlayerPrefs.SetInt($"{panelDataSO.gameName}", (level == totalAudio - 1) ? 0 : ++level);
    //     PlayerPrefs.Save();
    // }
}

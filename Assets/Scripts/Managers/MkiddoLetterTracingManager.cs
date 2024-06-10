using System.Collections;
using System.Collections.Generic;
using IndieStudio.EnglishTracingBook.Game;
using UnityEngine;

public class MkiddoLetterTracingManager : LevelBaseManager
{


    // Reference to the PanelDataSO scriptable object
    public PanelDataSO panelDataSO;


    public UIManager uIManager;



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void OnEnable()
    {
        level = PlayerPrefs.GetInt($"{panelDataSO.gameName}", 0);
        ShapesManager.Shape.selectedShapeID = level;
    }
}

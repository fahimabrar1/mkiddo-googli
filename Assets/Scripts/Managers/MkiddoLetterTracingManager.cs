using System;
using System.Collections;
using System.Collections.Generic;
using IndieStudio.EnglishTracingBook.Game;
using UnityEngine;

public class MkiddoLetterTracingManager : LevelBaseManager
{


    // Reference to the PanelDataSO scriptable object
    public PanelDataSO panelDataSO;


    public UIManager uIManager;

    public GameObject scrollViewContent;
    public GameObject buttonPrefab;
    public List<LetterHeaderButton> headerButtons;
    public Action<int> OnUpdateButtonAction;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void OnEnable()
    {
        AddButtons();
        level = PlayerPrefs.GetInt($"{panelDataSO.gameName}", 0);
        ShapesManager.Shape.selectedShapeID = level;
    }


    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        foreach (var button in headerButtons)
        {
            OnUpdateButtonAction -= button.OnUpdateButton;
        }
    }



    public void AddButtons()
    {
        if (panelDataSO.gamePanelData.blockID == 96)
        {
            // it's bangla learn_bangla_soroborno
            if (AlphabetLists.AlphabetTypes.TryGetValue("learn_bangla_soroborno", out List<string> sorbornos))
            {
                headerButtons = new();
                for (int i = 0; i < sorbornos.Count; i++)
                {
                    GameObject obj = Instantiate(buttonPrefab, scrollViewContent.transform);
                    if (obj.TryGetComponent(out LetterHeaderButton button))
                    {
                        headerButtons.Add(button);
                        button.mkiddoLetterTracingManager = this;
                        button.letterText.text = sorbornos[i];
                        OnUpdateButtonAction += button.OnUpdateButton;
                        button.ButtonID = i;
                    }
                }
            }


            OnUpdateButtonAction?.Invoke(level);


        }

    }
}

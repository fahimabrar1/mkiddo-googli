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
        level = PlayerPrefs.GetInt($"{panelDataSO.gameName}", 0);
        ShapesManager.Shape.selectedShapeID = level;
        AddButtons();
        StarCounts = 3;
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
            if (panelDataSO.gameName == "bangla_shorborno")
            {
                // it's bangla learn_bangla_soroborno
                if (AlphabetLists.AlphabetTypes.TryGetValue("learn_bangla_soroborno", out List<string> sorbornos))
                {
                    CreateHeaderbutton(sorbornos);
                }
            }
            else if (panelDataSO.gameName == "capital_letter_a_to_z")
            {
                // it's english uppercase  english_capital_letters
                if (AlphabetLists.AlphabetTypes.TryGetValue("english_capital_letters", out List<string> sorbornos))
                {
                    CreateHeaderbutton(sorbornos);
                }
            }
            OnUpdateButtonAction?.Invoke(level);
        }
    }



    private void CreateHeaderbutton(List<string> letters)
    {
        headerButtons = new();
        for (int i = 0; i < letters.Count; i++)
        {
            GameObject obj = Instantiate(buttonPrefab, scrollViewContent.transform);
            if (obj.TryGetComponent(out LetterHeaderButton button))
            {
                headerButtons.Add(button);
                button.mkiddoLetterTracingManager = this;
                button.letterText.text = letters[i];
                if (level > i)
                    button.SetCompletedBackground();
                OnUpdateButtonAction += button.OnUpdateButton;
                button.ButtonID = i;
            }
        }
    }
    public override void SaveLevel()
    {
        PlayerPrefs.SetInt($"{panelDataSO.gameName}", (level == headerButtons.Count - 1) ? 0 : ++level);
        PlayerPrefs.Save();
    }
}

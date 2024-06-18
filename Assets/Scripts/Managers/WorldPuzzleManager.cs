using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldPuzzleManager : LevelBaseManager
{

    public ButtonSelector buttonSelector;
    public TMP_Text questionText;

    public GameObject buttonContainerPrefab;
    public Transform buttonContainerParent;
    public DhadharuDataSo dhadharuDataSo;

    public Image OutlineBackgorund;
    public Image Backgorund;
    public Stack<LetterButton> buttons;
    public List<LetterContainer> containers;
    public List<Color> containerColors;

    public Color failed;
    public Color failedLowOp;
    public Color Success;
    public Color SuccessLowOp;
    public int TOTAL_BUTTONS_TO_BE_SELECTED = 0;
    public int CURRENT_SELECTED_BUTTONS = 0;

    public Action<int> OnTapLetterButtonAction;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        buttons = new();
        containers = new();
        level = PlayerPrefs.GetInt($"{dhadharuDataSo.gameName}", 0);
        var question = dhadharuDataSo.questions[level];
        buttonSelector.SetButtons(question.question_ans);
        TOTAL_BUTTONS_TO_BE_SELECTED = question.question_ans.Length;
        for (int i = 0; i < TOTAL_BUTTONS_TO_BE_SELECTED; i++)
        {
            GameObject buttonContainer = Instantiate(buttonContainerPrefab, buttonContainerParent);
            if (buttonContainer.TryGetComponent(out LetterContainer component))
            {
                component.ID = i;
                containers.Add(component);
            }
        }
        questionText.text = question.question_text;
    }


    public override void SaveLevel()
    {
        PlayerPrefs.SetInt($"{dhadharuDataSo.gameName}", (level == dhadharuDataSo.questions.Count - 1) ? 0 : ++level);
        PlayerPrefs.Save();
    }

    internal void AddLetterContianer(LetterButton letterButton)
    {

        containers[CURRENT_SELECTED_BUTTONS].SetButton(letterButton.id, GetColorByType(letterButton.letterButtonColorType), letterButton.letter.text, letterButton.letterButtonColorType);
        CURRENT_SELECTED_BUTTONS++;
        if (CURRENT_SELECTED_BUTTONS == TOTAL_BUTTONS_TO_BE_SELECTED)
        {
            //Todo: Check for game
            string containerText = "";
            foreach (var item in containers)
            {
                containerText += item.letter.text;
            }
            if (containerText == dhadharuDataSo.questions[level].question_ans)
            {
                OutlineBackgorund.color = Success;
                Backgorund.color = SuccessLowOp;
            }
            else
            {
                OutlineBackgorund.color = failed;
                Backgorund.color = failedLowOp;
            }
        }
    }
    internal void RemoveContainer(int id)
    {
        containers.Where((a) => a.LetterID == id).FirstOrDefault().ResetButton();
        CURRENT_SELECTED_BUTTONS--;
    }

    private Color GetColorByType(LetterButtonColorType letterButtonColorType)
    {
        switch (letterButtonColorType)
        {
            case LetterButtonColorType.leaf:
                return containerColors[1];
            case LetterButtonColorType.pink:
                return containerColors[2];
            case LetterButtonColorType.orange:
                return containerColors[3];
            case LetterButtonColorType.yellow:
                return containerColors[4];
            default:
                return containerColors[0];
        }
    }


}

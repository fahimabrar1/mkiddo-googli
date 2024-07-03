using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldPuzzleManager : LevelBaseManager
{

    public ButtonSelector buttonSelector;
    public TMP_Text questionText;

    public DhadahruLevelAudioManahger dhadahruLevelAudioManahger;
    public GameObject Body;
    public GameObject buttonContainerPrefab;
    public Transform buttonContainerParent;
    public Transform GameEndbuttonContainerParent;
    public DhadharuDataSo dhadharuDataSo;

    public Image OutlineBackgorund;
    public Image Backgorund;
    public GameManager gameManager;
    public Stack<LetterButton> buttons;
    public List<LetterContainer> containers;
    public List<LetterContainer> GameEndContainers;
    public List<Color> containerColors;

    public Color failed;
    public Color failedLowOp;
    public Color Success;
    public Color SuccessLowOp;
    public int TOTAL_BUTTONS_TO_BE_SELECTED = 0;
    public int CURRENT_SELECTED_BUTTONS = 0;

    public int tempLevel = -1;
    [Header("Game Over Panels")]
    public ProgressBarTimer timer;
    public GameObject ContentPanel;
    public GameObject GameOverPanel;

    public Button nextButton;
    public Button backButton;

    public Action<int> OnTapLetterButtonAction;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        buttons = new();
        containers = new();
        level = 3;
        gameManager = FindAnyObjectByType<GameManager>();
        level = PlayerPrefs.GetInt($"{dhadharuDataSo.gameName}", 0);
        tempLevel = PlayerPrefs.GetInt($"{dhadharuDataSo.gameName}_temp", 0);
        if (tempLevel == -1)
        {
            tempLevel = level;
        }
        MyDebug.Log($"Temp Level: {tempLevel}");
        MyDebug.Log($" Level: {level}");
        nextButton.gameObject.SetActive(tempLevel != -1 && tempLevel < level);
        backButton.gameObject.SetActive(level > 0 && level < dhadharuDataSo.questions.Count);

        var question = dhadharuDataSo.questions[tempLevel];
        TOTAL_BUTTONS_TO_BE_SELECTED = question.question_ans.Length;
        for (int i = 0; i < TOTAL_BUTTONS_TO_BE_SELECTED; i++)
        {
            GameObject buttonContainer = Instantiate(buttonContainerPrefab, buttonContainerParent);
            if (buttonContainer.TryGetComponent(out LetterContainer component))
            {
                component.ID = i;
                containers.Add(component);
            }

            GameObject buttonContainer1 = Instantiate(buttonContainerPrefab, GameEndbuttonContainerParent);
            if (buttonContainer1.TryGetComponent(out LetterContainer component1))
            {
                component1.ID = i;
                GameEndContainers.Add(component1);
            }
        }
        buttonSelector.SetButtons(question.question_ans);

        questionText.text = question.question_text;
    }




    internal void AddLetterContianer(LetterButton letterButton)
    {

        containers[CURRENT_SELECTED_BUTTONS].SetButton(letterButton.id, GetColorByType(letterButton.letterButtonColorType), letterButton.letter.text, letterButton.letterButtonColorType);
        CURRENT_SELECTED_BUTTONS++;
        if (CURRENT_SELECTED_BUTTONS == TOTAL_BUTTONS_TO_BE_SELECTED)
        {

            string containerText = "";
            foreach (var item in containers)
            {
                containerText += item.letter.text;
            }

            // Win
            if (containerText == dhadharuDataSo.questions[tempLevel].question_ans)
            {
                timer.Stop();

                OutlineBackgorund.color = Success;
                Backgorund.color = SuccessLowOp;
                OnGameOver();
            }
            else
            {
                // Lose
                OutlineBackgorund.color = failed;
                Backgorund.color = failedLowOp;
            }


        }
    }



    public async void OnGameOver(bool isTimeUp = false)
    {
        await Task.Delay(1000);
        if (!isTimeUp)
            dhadahruLevelAudioManahger.PlayRandomPraise();
        Body.SetActive(false);
        ContentPanel.SetActive(true);
        UpdateGameEndContent(buttonSelector.letterButtons);
        await Task.Delay(2000);
        ContentPanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }


    public void UpdateGameEndContent(List<LetterButton> letterButtons)
    {

        for (int i = 0; i < dhadharuDataSo.questions[tempLevel].question_ans.Length; i++)
        {
            var character = dhadharuDataSo.questions[tempLevel].question_ans[i];
            var letter = letterButtons.FirstOrDefault((l) => l.letter.text == character.ToString());
            GameEndContainers[i].EnableButton();
            GameEndContainers[i].SetButton(letter.id, GetColorByType(letter.letterButtonColorType), letter.letter.text, letter.letterButtonColorType);
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



    public void OnClickBack()
    {
        tempLevel--;
        if (tempLevel >= 0)
        {
            PlayerPrefs.SetInt($"{dhadharuDataSo.gameName}_temp", tempLevel);
            PlayerPrefs.Save();

            gameManager.RestartLevel();
        }
    }

    public void OnClickNext()
    {
        SaveLevel();
        gameManager.RestartLevel();
    }



    public override void SaveLevel()
    {
        if (tempLevel == level)
        {
            level++;
            PlayerPrefs.SetInt($"{dhadharuDataSo.gameName}", (level == dhadharuDataSo.questions.Count - 1) ? 0 : level);
            PlayerPrefs.SetInt($"{dhadharuDataSo.gameName}_temp", -1);
        }
        else
        {
            tempLevel++;
            PlayerPrefs.SetInt($"{dhadharuDataSo.gameName}_temp", tempLevel);
        }

        PlayerPrefs.Save();
    }

}

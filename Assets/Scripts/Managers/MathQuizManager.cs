using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MathQuizManager : LevelBaseManager
{

    public MathQuizButtonSelector buttonSelector;
    public Image questionImage;

    public DhadahruLevelAudioManahger dhadahruLevelAudioManahger;
    public DhadharuDataSo dhadharuDataSo;

    public GameManager gameManager;

    public int tempLevel = -1;
    [Header("Game Over Panels")]
    public ProgressBarTimer timer;
    public GameObject GameOverPanel;
    private MyWebRequest myWebRequest;

    public Button nextButton;
    public Button backButton;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    async void Start()
    {

        myWebRequest = new();
        level = 3;
        gameManager = FindAnyObjectByType<GameManager>();
        level = PlayerPrefs.GetInt($"{dhadharuDataSo.gameName}", 0);
        tempLevel = PlayerPrefs.GetInt($"{dhadharuDataSo.gameName}_temp", 0);
        if (tempLevel == -1)
        {
            tempLevel = level;
        }


        nextButton.gameObject.SetActive(tempLevel < level);
        backButton.gameObject.SetActive(tempLevel > 0 && tempLevel <= level);

        var question = dhadharuDataSo.questions[tempLevel];
        Sprite sprite = await myWebRequest.FetchImageAsync(dhadharuDataSo.questions[tempLevel].question_image);

        questionImage.sprite = sprite;
        Color col = questionImage.color;
        col.a = 1;
        questionImage.color = col;
        buttonSelector.SetOptionButtons(question.question_ans);
    }



    public async void OnGameOver(bool isTimeUp = false)
    {
        timer.Stop();
        if (!isTimeUp)
            dhadahruLevelAudioManahger.PlayRandomPraise();
        await Task.Delay(1000);
        GameOverPanel.SetActive(true);
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

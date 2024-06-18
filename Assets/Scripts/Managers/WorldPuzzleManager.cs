using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPuzzleManager : LevelBaseManager
{

    public ButtonSelector buttonSelector;
    public DhadharuDataSo dhadharuDataSo;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        level = PlayerPrefs.GetInt($"{dhadharuDataSo.gameName}", 0);
        var question = dhadharuDataSo.questions[level];
        buttonSelector.SetButtons(question.question_ans);
    }


    public override void SaveLevel()
    {
        PlayerPrefs.SetInt($"{dhadharuDataSo.gameName}", (level == dhadharuDataSo.questions.Count - 1) ? 0 : ++level);
        PlayerPrefs.Save();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriviaButtonSelector : MonoBehaviour
{
    public TriviaQuizManager triviaQuizManager;
    public List<TriviaOptionButton> triviaOptionButtons;

    public Action OnResetButtonAction;
    public Action OnDisableAllButtonAction;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        foreach (var btn in triviaOptionButtons)
        {
            OnResetButtonAction += btn.OnResetButton;
            OnDisableAllButtonAction += btn.OnDisableButton;
        }
    }




    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        foreach (var btn in triviaOptionButtons)
        {
            OnResetButtonAction -= btn.OnResetButton;
            OnDisableAllButtonAction -= btn.OnDisableButton;
        }
    }

    internal void SetOptionButtons(string question_ans, string question_option)
    {
        List<string> options = ParseInputString(question_option);
        options.Shuffle();
        for (int i = 0; i < options.Count; i++)
        {
            var splits = options[i].Split(' ');
            triviaOptionButtons[i].triviaButtonSelector = this;
            triviaOptionButtons[i].answer = question_ans;
            triviaOptionButtons[i].option.text = splits[1];
            triviaOptionButtons[i].prefix = splits[0];
        }
    }

    List<string> ParseInputString(string input)
    {
        // Remove the square brackets and quotes, then split by commas
        string cleanedInput = input.Replace("[", "").Replace("]", "").Replace("\"", "").Trim();
        string[] splitInput = cleanedInput.Split(new string[] { ", " }, System.StringSplitOptions.None);

        return new List<string>(splitInput);
    }

    internal void OnGameOver()
    {
        triviaQuizManager.OnGameOver();
    }

}

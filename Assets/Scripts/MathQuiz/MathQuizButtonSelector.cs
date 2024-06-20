using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MathQuizButtonSelector : MonoBehaviour
{
    public MathQuizManager mathQuizManager;
    public GameObject buttonPrefab;
    public MathQuizAnswer mathQuizAnswer;
    public List<MathOptionButton> mathQuizOptionButtons;

    public Action OnResetButtonAction;
    public Action OnDisableAllButtonAction;




    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        foreach (var btn in mathQuizOptionButtons)
        {
            OnResetButtonAction -= btn.OnResetButton;
            OnDisableAllButtonAction -= btn.OnDisableButton;
        }
        OnResetButtonAction -= mathQuizAnswer.OnResetAnswer;

    }

    internal void SetOptionButtons(string question_ans)
    {
        mathQuizAnswer.asnwer = question_ans;
        mathQuizAnswer.OnResetAnswer();

        int rand = UnityEngine.Random.Range(3, 5);
        int answerRand = UnityEngine.Random.Range(0, rand);

        for (int i = 0; i < rand; i++)
        {
            var obj = Instantiate(buttonPrefab, transform);
            if (obj.TryGetComponent(out MathOptionButton mathOption))
            {
                mathOption.mathQuizButtonSelector = this;
                mathOption.id = i;
                mathOption.answer = question_ans;

                if (answerRand == i)
                {
                    mathOption.option.text = question_ans;
                }
                else
                {
                    mathOption.option.text = GetRandomValue(int.Parse(question_ans)).ToString();
                }

                mathQuizOptionButtons.Add(mathOption);
            }
        }

        foreach (var btn in mathQuizOptionButtons)
        {
            OnResetButtonAction += btn.OnResetButton;
            OnDisableAllButtonAction += btn.OnDisableButton;
        }

        OnResetButtonAction += mathQuizAnswer.OnResetAnswer;

        OnResetButtonAction?.Invoke();
    }


    public int GetRandomValue(int originalValue)
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand == originalValue)
            return GetRandomValue(originalValue);
        return rand;

    }

    public void OnGameOver(string answer)
    {
        mathQuizAnswer.OnShowAnswer(answer);
        mathQuizManager.OnGameOver();
    }


    public void OnTapWrongAnswer(string answer)
    {
        mathQuizAnswer.OnShowAnswer(answer);

    }

}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MathQuizAnswer : MonoBehaviour
{
    public string asnwer;
    public Image background;
    public TMP_Text answerText;
    public Sprite correntSprite;
    public Sprite wrongSprite;



    public void OnShowAnswer(string asnwer)
    {

        answerText.text = asnwer;
        if (this.asnwer == asnwer)
        {
            background.sprite = correntSprite;
        }
        else
        {
            background.sprite = wrongSprite;
        }
        OnEnableAnswer();
    }


    public void OnResetAnswer()
    {
        background.enabled = false;
        answerText.enabled = false;
    }

    public void OnEnableAnswer()
    {
        background.enabled = true;
        answerText.enabled = true;
    }
}

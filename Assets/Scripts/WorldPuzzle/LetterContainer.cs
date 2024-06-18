using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterContainer : MonoBehaviour
{

    public int ID;
    public int LetterID = -1;
    public TMP_Text letter;
    public Image background;
    public Image coloredBackground;


    public void SetButton(int id, Color color, string txt, LetterButtonColorType letterButtonColorType)
    {
        LetterID = id;
        coloredBackground.color = color;
        letter.text = txt;

        if (letterButtonColorType == LetterButtonColorType.yellow)
            letter.color = Color.white;
        else
            letter.color = Color.black;
        EnableButton();
    }


    public void ResetButton()
    {
        LetterID = -1;
        background.enabled = false;
        coloredBackground.enabled = false;
        letter.enabled = false;
    }


    public void EnableButton()
    {
        background.enabled = true;
        coloredBackground.enabled = true;
        letter.enabled = true;
    }
}

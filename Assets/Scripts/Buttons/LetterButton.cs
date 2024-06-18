using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterButton : MonoBehaviour
{
    public int id;
    public LetterButtonColorType letterButtonColorType;
    public Button button;
    public TMP_Text letter;
    public bool isSelected;

    internal void Initialize(char l)
    {
        isSelected = false;
        letter.text = l.ToString();
    }

    public void OnClickButton()
    {
        isSelected = !isSelected;
        if (isSelected)
            transform.localScale = new(1.2f, 1.2f, 1.2f);
        else
            transform.localScale = Vector3.one;
    }


    public void ResetButton()
    {
        isSelected = false;
        transform.localScale = Vector3.one;
    }
}


public enum LetterButtonColorType
{
    green,
    leaf,
    pink,
    orange,
    yellow,
}

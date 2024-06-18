using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterButton : MonoBehaviour
{
    int id;
    public Button button;
    public TMP_Text letter;

    internal void Initialize(char l)
    {
        letter.text = l.ToString();
    }

}

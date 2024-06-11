using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterHeaderButton : MonoBehaviour
{

    public int ButtonID;
    public GameObject backgroundOverlay;
    public Image ButtonImage;
    public TMP_Text letterText;
    public Color buttonBGFinishedColor;

    public MkiddoLetterTracingManager mkiddoLetterTracingManager;

    private bool isSelected;
    private bool isCompleted;


    public void OnButtonClick()
    {
        mkiddoLetterTracingManager.OnUpdateButtonAction?.Invoke(ButtonID);
    }



    public void OnUpdateButton(int id)
    {
        if (id == ButtonID)
        {
            isSelected = true;
            backgroundOverlay.SetActive(true);
        }
        else
        {

            isSelected = false;
            backgroundOverlay.SetActive(false);
        }
    }
}


using System;
using UnityEngine;

[Serializable]
public class LoginScreenModel
{
    public GameObject panel;
    public GameObject backButton;
    public GameObject nextButton;
    public GameObject playButton;


    public void SetActive(bool val)
    {
        panel.SetActive(val);
        if (backButton != null)
            backButton.SetActive(val);
        if (nextButton != null)
            nextButton.SetActive(val);
        if (playButton != null)
            playButton.SetActive(val);
    }
}
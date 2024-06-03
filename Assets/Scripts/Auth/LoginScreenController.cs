using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static StringExtensions;

public class LoginScreenController : MonoBehaviour
{
    public int currentPanelIndex = 0;
    public int maxPanelIndex = 0;
    public ProfileSO profileSO;

    public Button back;
    public Button next;
    public Button play;
    public List<GameObject> panels;





    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        maxPanelIndex = panels.Count;
        currentPanelIndex = 0;
        OnUpdatePanel();
    }


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void OnUpdatePanel()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            if (i == currentPanelIndex)
                panels[i].SetActive(true);
            else
                panels[i].SetActive(false);
        }

        if (currentPanelIndex == 0)
        {
            next.gameObject.SetActive(false);
            back.gameObject.SetActive(false);
        }
        else if (currentPanelIndex > 0 && currentPanelIndex < maxPanelIndex - 1)
        {
            next.gameObject.SetActive(true);
            back.gameObject.SetActive(true);
        }
        else if (currentPanelIndex == maxPanelIndex - 1)
        {
            play.gameObject.SetActive(true);
            next.gameObject.SetActive(false);
            back.gameObject.SetActive(true);
        }
    }


    public void OnClickNext()
    {
        MyDebug.Log("On Change Panel");
        currentPanelIndex++;
        OnUpdatePanel();
    }
    public void OnClickBack()
    {
        currentPanelIndex--;
        OnUpdatePanel();

    }
    public void OnClickPlay()
    {
        SceneManager.LoadSceneAsync(1);
    }
}

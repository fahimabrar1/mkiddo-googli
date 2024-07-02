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

    // public Button play;
    public List<GameObject> panels;


    int loggedIn = 0;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        loggedIn = PlayerPrefs.GetInt("is_logged_in", 0);
        if (loggedIn == 1)
            OnClickPlay();
        else
            panels[0].SetActive(true);
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

        // if (currentPanelIndex == 0)
        // {
        //     next.gameObject.SetActive(false);
        //     back.gameObject.SetActive(false);
        // }
        // else if (currentPanelIndex > 0 && currentPanelIndex < maxPanelIndex - 1)
        // {
        //     next.gameObject.SetActive(true);
        //     back.gameObject.SetActive(true);
        // }
        // else if (currentPanelIndex == maxPanelIndex - 1)
        // {
        //     play.gameObject.SetActive(true);
        //     next.gameObject.SetActive(false);
        //     back.gameObject.SetActive(true);
        // }
    }


    public void OnClickNext()
    {
        MyDebug.Log("On Change Panel");
        currentPanelIndex++;
        OnUpdatePanel();
    }


    public void OnJumpTo(int index)
    {
        MyDebug.Log("On Change Panel");
        currentPanelIndex = index;
        OnUpdatePanel();
    }
    public void OnClickBack()
    {
        if (currentPanelIndex == 0) return;
        currentPanelIndex--;
        OnUpdatePanel();
    }

    // public void OnToggleNextButton(bool val)
    // {
    //     next.gameObject.SetActive(val);
    // }

    // public void OnToggleBackButton(bool val)
    // {
    //     back.gameObject.SetActive(val);
    // }


    public void OnClickPlay()
    {
        SceneManager.LoadSceneAsync(1);
    }
}

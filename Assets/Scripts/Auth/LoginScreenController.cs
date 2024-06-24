using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google;
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

    public Button play;
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
        if (currentPanelIndex < 0 || currentPanelIndex >= panels.Count)
        {
            Debug.LogWarning("currentPanelIndex is out of range!");
            return;
        }

        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(false);
        }

        panels[currentPanelIndex].SetActive(true);
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

    public string webClientId = "794416134528-8l6qfs5vg5om3v8q9c9reqel25ovj2ck.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true
        };
    }

    public void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddStatusText("Calling SignIn");
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished, TaskScheduler.Default);
    }

    public void OnSignOut()
    {
        AddStatusText("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        AddStatusText("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        MyDebug.Log($"Google: {task.Result}");
        if (task.IsFaulted)
        {
            using IEnumerator<Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator();
            if (enumerator.MoveNext())
            {
                GoogleSignIn.SignInException error =
                        (GoogleSignIn.SignInException)enumerator.Current;
                AddStatusText("Got Error: " + error.Status + " " + error.Message);
            }
            else
            {
                AddStatusText("Got Unexpected Exception?!?" + task.Exception);
            }
        }
        else if (task.IsCanceled)
        {
            AddStatusText("Canceled");
        }
        else
        {
            profileSO.childName = task.Result.DisplayName;
            profileSO.isSignUsingGoogle = true;
            profileSO.ImageURI = task.Result.ImageUrl;
            PlayerPrefs.SetString("access_token", task.Result.IdToken);
            PlayerPrefs.Save();
            AddStatusText("Welcome: " + task.Result.DisplayName + "!");


            MyDebug.Log("Welcome: " + task.Result.DisplayName + "!");

            OnJumpTo(3);

        }
    }

    private void OnJumpTo(int v)
    {
        currentPanelIndex = v;
        OnUpdatePanel();
    }


    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddStatusText("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently()
              .ContinueWith(OnAuthenticationFinished);
    }


    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddStatusText("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);
    }

    private List<string> messages = new List<string>();
    void AddStatusText(string text)
    {
        if (messages.Count == 5)
        {
            messages.RemoveAt(0);
        }
        messages.Add(text);
        string txt = "";
        foreach (string s in messages)
        {
            txt += "\n" + s;
        }

        MyDebug.Log($"Google: {text}");

    }
}

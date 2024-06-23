using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Diagnostics;
using DG.Tweening;
using Google;
using System.Threading.Tasks;
public class LoginPanelOne : LoginPanelBase
{


    public TMP_Dropdown numberDropdown;
    public int numberDropdownIndex = 0;
    public Button Continuee;
    public Button SignInWithGoogle;

    public TMP_InputField numberText;
    public TMP_Text textLimitText;
    public TMP_Text warningText;

    MyWebRequest myWebRequest;



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {


        if (PlayerPrefs.GetInt("logged_in", 0) == 1)
        {
            FindObjectOfType<GameManager>().LoadSceneAsync(1);
        }
        myWebRequest = new();
        // Create a list to hold the dropdown options
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        // Iterate through the countryMobileCodes and create OptionData objects
        foreach (string code in Utility.CountryDetails.countryMobileCodes)
        {
            TMP_Dropdown.OptionData option = new(code);
            options.Add(option);
        }


        // Add the options to the dropdown
        numberDropdown.AddOptions(options);
        numberDropdown.value = 11;
        OnUpdateCountryCode(11);
        Continuee.interactable = false;
    }



    public void OnTapDialerButton(string num)
    {
        Debug.Log(num);
        if (numberText.text.Length <= 10 && num.Length > 0)
        {
            numberText.text = num.ToString();
            loginScreenController.profileSO.mobileNumber = numberText.text;
            textLimitText.text = numberText.text.Length + "/10";
        }
        else if (num.Length == 0)
        {
            loginScreenController.profileSO.mobileNumber = "";
            textLimitText.text = "0/10";
        }

        if (num.Length == 10)
        {
            Continuee.interactable = true;
        }
        else
        {
            Continuee.interactable = false;
        }
    }


    public void OnUpdateCountryCode(int value)
    {
        var str = Utility.CountryDetails.countryMobileCodes[value];
        var values = str.Split(' ');
        var code = values[1].Replace("+", "");
        loginScreenController.profileSO.countryCode = code;
    }



    public void OnTapNext()
    {
        myWebRequest.SendOTP("/api/v2/send-otp", loginScreenController.profileSO.countryCode + loginScreenController.profileSO.mobileNumber, OnSuccessSendNumber, OnFailedSendNumber);

        // {"success":false,"status_code":402,"message":"INVALID MSISDN"}
    }


    public void OnSuccessSendNumber(MyWebReqSuccessCallback result)
    {
        if (result.status_code == 200)
        {

            loginScreenController.OnClickNext();
            gameObject.SetActive(false);

        }
        else if (result.status_code == 402)
        {
            warningText.text = "Invalid Number";
            warningText.color = failedColor;
            warningText.gameObject.SetActive(true);
            warningText.gameObject.transform.DOScale(Vector3.one, 0.2f).SetAutoKill(true);
        }

    }
    public void OnFailedSendNumber(MyWebReqFailedCallback result)
    {
        warningText.text = result.message;
        warningText.color = successColor;
        warningText.gameObject.SetActive(true);
        warningText.gameObject.transform.DOScale(Vector3.one, 0.2f).SetAutoKill(true);
    }

    #region Google


    public string webClientId = "160760181826-5lupkrn266m01qd1u4jqtnlm251hhobu.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;
    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true,
            RequestEmail = true,

        };
    }
    public void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;
        AddStatusText("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);
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
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
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
        }
        else if (task.IsCanceled)
        {
            AddStatusText("Canceled");
        }
        else
        {

            loginScreenController.profileSO.childName = task.Result.DisplayName;
            loginScreenController.profileSO.isSignUsingGoogle = true;
            loginScreenController.profileSO.ImageURI = task.Result.ImageUrl;

            loginScreenController.OnJumpTo(3);
            AddStatusText("Welcome: " + task.Result.DisplayName + "!");
            AddStatusText("TokenID: " + task.Result.IdToken + "!");
        }
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
    }

    #endregion
}
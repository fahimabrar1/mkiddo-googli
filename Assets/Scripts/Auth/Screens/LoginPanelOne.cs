using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Diagnostics;
using DG.Tweening;
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
}
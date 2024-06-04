using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Diagnostics;
using DG.Tweening;
public class LoginPanelOne : LoginPanelBase
{
    public Button Back;
    public Button Next;
    public GameObject NextPanel;

    public List<GameObject> panels;

    [Header("Panel One")]
    public TMP_Dropdown numberDropdown;
    public int numberDropdownIndex = 0;
    public Button TypeHereButton;
    public Button SignInWithGoogle;
    [Header("Panel Two")]

    public TMP_Dropdown numberDropdown1;
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
        numberDropdown1.AddOptions(options);
        numberDropdown.value = 11;
        numberDropdown1.value = 11;
        OnUpdateCountryCode(11);
    }


    public void OnTapDialerButton(int num)
    {

        if (numberText.text.Length <= 10)
        {
            numberText.text += num.ToString();
            loginScreenController.profileSO.mobileNumber = numberText.text;
            textLimitText.text = numberText.text.Length + "/10";
            Next.gameObject.SetActive(true);
        }

    }
    public void OnTapDialerButton(string num)
    {
        Debug.Log(num);
        if (numberText.text.Length <= 10 && num.Length > 0)
        {
            numberText.text = num.ToString();
            loginScreenController.profileSO.mobileNumber = numberText.text;
            textLimitText.text = numberText.text.Length + "/10";
            Next.gameObject.SetActive(true);
        }
        else if (num.Length == 0)
        {
            loginScreenController.profileSO.mobileNumber = "";
            textLimitText.text = "0/10";
            Next.gameObject.SetActive(false);
        }
    }

    public void OnTapNumberPanel()
    {
        panels[0].SetActive(false);
        panels[1].SetActive(true);
    }


    public void OnTapDialerBack()
    {
        Next.gameObject.SetActive(false);
        loginScreenController.profileSO.mobileNumber = "";
        textLimitText.text = "0/10";
        numberText.text = "";

        if (warningText.gameObject.activeInHierarchy)
        {
            warningText.text = "";
            warningText.color = successColor;
            warningText.gameObject.SetActive(false);
            warningText.gameObject.transform.DOScale(Vector3.zero, 0.2f).SetAutoKill(true);
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

            NextPanel.SetActive(true);
            gameObject.SetActive(false);
        }
        else if (result.status_code == 402)
        {
            warningText.text = "Invalid Number";
            warningText.color = failedColor;
            warningText.gameObject.SetActive(true);
            warningText.gameObject.transform.DOScale(Vector3.one, 0.2f).SetAutoKill(true);
            Next.gameObject.SetActive(false);
        }

    }
    public void OnFailedSendNumber(MyWebReqFailedCallback result)
    {
        warningText.text = result.message;
        warningText.color = successColor;
        warningText.gameObject.SetActive(true);
        warningText.gameObject.transform.DOScale(Vector3.one, 0.2f).SetAutoKill(true);
        Next.gameObject.SetActive(false);
    }
}
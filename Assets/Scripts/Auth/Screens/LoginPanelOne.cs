using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Diagnostics;
using DG.Tweening;
public class LoginPanelOne : LoginPanelBase
{
    public Button Next;
    public GameObject NextPanel;

    public Sprite InputField1;
    public Sprite InputField2;
    public Image inputImage;

    public List<GameObject> panels;

    [Header("Panel One")]
    public int numberDropdownIndex = 0;
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
        List<TMP_Dropdown.OptionData> options = new();

        // Iterate through the countryMobileCodes and create OptionData objects
        foreach (string code in Utility.CountryDetails.countryMobileCodes)
        {
            TMP_Dropdown.OptionData option = new(code);
            options.Add(option);
        }


        // Add the options to the dropdown
        numberDropdown1.AddOptions(options);
        numberDropdown1.value = 11;
        OnUpdateCountryCode(11);

        Next.gameObject.SetActive(false);
    }


    public void OnTapDialerButton(string num)
    {
        MyDebug.Log(num);

        if (num.Length > 0 && num[0] == '0')
        {
            numberText.text = "";
            textLimitText.text = "0/10";
            return;
        }
        if (numberText.text.Length <= 10 && num.Length > 0)
        {
            numberText.text = num.ToString();
            loginScreenController.profileSO.mobileNumber = numberText.text;
            textLimitText.text = numberText.text.Length + "/10";
            if (numberText.text.Length < 10)
            {
                inputImage.sprite = InputField1;
                Next.gameObject.SetActive(false);
            }
            else
            {
                inputImage.sprite = InputField2;
                Next.gameObject.SetActive(true);
            }
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
            Next.gameObject.SetActive(false);
            loginScreenController.OnClickNext();
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
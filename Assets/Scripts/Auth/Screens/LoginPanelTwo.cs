using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class LoginPanelTwo : LoginPanelBase
{
    public Button Back;
    public Button Next;
    public TMP_Text headerText;
    public TMP_Text resultText;
    public TMP_Text resendCodeText;
    public TMP_InputField inputField;
    public List<TMP_Text> PinFields;
    private int currentIndex = 0;
    public Button resendButton;
    public Color greyColor;
    public int resendCooldown = 60; // Time in seconds to wait before enabling the resend button

    MyWebRequest myWebRequest;
    private string otp = "";



    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        myWebRequest = new();
        resendCodeText.text = "Resend";
        resendButton.interactable = true;
        resendCodeText.color = Color.black;

        otp = "";
        loginScreenController.profileSO.id = -1;
        loginScreenController.profileSO.name = "";
        loginScreenController.profileSO.day = "";
        loginScreenController.profileSO.month = "";
        loginScreenController.profileSO.year = "";
        loginScreenController.profileSO.avatarPath = "";
        headerText.text = $"Enter your four digit code that we have sent to your mobile number ({loginScreenController.profileSO.countryCode} {loginScreenController.profileSO.mobileNumber})";

        if (inputField.text.Length > 0)
            Next.gameObject.SetActive(false);
        Back.gameObject.SetActive(true);


        resendButton.onClick.AddListener(OnClickResendCode);
    }


    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        resendButton.onClick.RemoveAllListeners();
    }

    void Start()
    {
        resultText.gameObject.transform.localScale = Vector3.zero;
        resultText.gameObject.SetActive(false);
        // Ensure all input fields are cleared initially
        foreach (var inputField in PinFields)
        {
            inputField.text = "";
        }

        // Assign the resend button click event
        resendButton.onClick.AddListener(OnClickResendCode);
        Next.gameObject.SetActive(false);
    }


    public void OnSuccessCallback(MkiddOOnVerificationSuccessModel result)
    {
        if (result.status_code == 200)
        {
            Next.gameObject.SetActive(false);
            resultText.text = "Success";
            resultText.color = successColor;
            resultText.gameObject.SetActive(true);
            resultText.gameObject.transform.DOScale(Vector3.one, 0.2f).SetAutoKill(true);
            FileProcessor fileProcessor = new();
            UpdateUserData(result);
            fileProcessor.SaveJsonToFile(fileProcessor.userFilePath, JsonUtility.ToJson(result));
            Debug.Log("Before Saved Token");

            MyPlayerPrefabs.Instance.SetString("access_token", result.accessToken);
            Debug.Log("Saved Token");

            var a = MyPlayerPrefabs.Instance.GetString("access_token");
            Debug.Log("After Fetching  Token" + a);
            loginScreenController.OnClickNext();
        }
        else
        {
            Next.gameObject.SetActive(false);
            resultText.text = "Invalid OTP, Try Again";
            resultText.color = failedColor;
            resultText.gameObject.SetActive(true);
            resultText.gameObject.transform.DOScale(Vector3.one, 0.2f).SetAutoKill(true);
        }
    }

    private void UpdateUserData(MkiddOOnVerificationSuccessModel result)
    {
        if (result.child_info.Length > 0)
        {
            var dates = result.child_info[0].birth_date.Split('-');

            loginScreenController.profileSO.id = result.uid;
            loginScreenController.profileSO.child_id = result.child_info[0].child_id;

            loginScreenController.profileSO.year = dates[0];
            loginScreenController.profileSO.month = dates[1].StartsWith("0") == true ? dates[1][1..] : dates[1];
            loginScreenController.profileSO.day = dates[2].StartsWith("0") == true ? dates[2][1..] : dates[2];

            loginScreenController.profileSO.childName = result.child_info[0].name;
            loginScreenController.profileSO.avatarPath = result.child_info[0].profile_path;
        }
    }
    public void OnFailedCallback(MyWebReqFailedCallback result)
    {
        resultText.text = result.message;
        resultText.color = failedColor;
        resultText.gameObject.SetActive(true);
        resultText.gameObject.transform.DOScale(Vector3.one, 0.2f).SetAutoKill(true);
    }


    public void OnInputValueChange(string val)
    {
        // Update PinFields
        currentIndex = val.Length;
        for (int i = 0; i < PinFields.Count; i++)
        {
            if (i < val.Length)
            {
                PinFields[i].text = val[i].ToString();
            }
            else
            {
                PinFields[i].text = string.Empty; // Clear any remaining fields
            }
        }

        if (currentIndex == PinFields.Count)
        {
            otp = "";
            foreach (var text in PinFields)
            {
                otp += text.text;
            }

            Next.gameObject.SetActive(true);
        }
        else
        {
            if (Next.gameObject.activeInHierarchy)
                Next.gameObject.SetActive(true);
        }
    }

    public void InputButton(int value)
    {
        if (currentIndex >= PinFields.Count)
        {
            return; // All fields are filled
        }

        // Update the current input field with the value
        PinFields[currentIndex].text = value.ToString();

        // Move to the next input field
        currentIndex++;
        if (currentIndex == PinFields.Count)
        {
            string otp = "";
            foreach (var text in PinFields)
            {
                otp += text.text;
            }

            StartCoroutine(myWebRequest.VerifyOTP("/api/V3/auth/sign-in", loginScreenController.profileSO.countryCode + loginScreenController.profileSO.mobileNumber, otp, OnSuccessCallback, OnFailedCallback));
        }
    }

    public void OnTapNext()
    {
        StartCoroutine(myWebRequest.VerifyOTP("/api/V3/auth/sign-in", loginScreenController.profileSO.countryCode + loginScreenController.profileSO.mobileNumber, otp, OnSuccessCallback, OnFailedCallback));
    }


    public void ClearAllFields()
    {

        if (resultText.gameObject.activeSelf)
            resultText.gameObject.transform.DOScale(Vector3.zero, 0.2f).SetAutoKill(true).OnComplete(() => resultText.gameObject.SetActive(false));
        foreach (var inputField in PinFields)
        {
            inputField.text = "";
        }

        // Reset the index and select the first input field
        currentIndex = 0;

    }

    public void OnClickResendCode()
    {
        if (!resendButton.interactable)
        {
            return; // If the button is already disabled, do nothing
        }

        resendCodeText.text = "Code resent!";
        string otp = "";
        foreach (var text in PinFields)
        {
            otp += text.text;
        }
        StartCoroutine(myWebRequest.SendOTP("/api/v2/send-otp", loginScreenController.profileSO.countryCode + loginScreenController.profileSO.mobileNumber, null, null));
        StartCoroutine(ResendCooldownCoroutine());
    }

    private IEnumerator ResendCooldownCoroutine()
    {
        resendButton.interactable = false;
        resendCodeText.color = greyColor;
        float remainingTime = resendCooldown;

        while (remainingTime > 0)
        {
            resendCodeText.text = $"Resend ({remainingTime})";
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        resendCodeText.text = "Resend";
        resendButton.interactable = true;
        resendCodeText.color = Color.black;
    }


}
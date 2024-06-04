using UnityEngine;
using TMPro;
using System.Diagnostics.Tracing;
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


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        myWebRequest = new();
        resendCodeText.text = "Resend";
        resendButton.interactable = true;
        resendCodeText.color = Color.black;

        if (inputField.text.Length > 0)
            Next.gameObject.SetActive(false);
    }


    void Start()
    {
        headerText.text = $"Enter your four digit code that we have sent to your mobile number ({loginScreenController.profileSO.countryCode} {loginScreenController.profileSO.mobileNumber})";
        resultText.gameObject.transform.localScale = Vector3.zero;
        resultText.gameObject.SetActive(false);
        // Ensure all input fields are cleared initially
        foreach (var inputField in PinFields)
        {
            inputField.text = "";
        }

        // Assign the resend button click event
        resendButton.onClick.AddListener(OnClickResendCode);
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
            myWebRequest.VerifyOTP("/api/V3/auth/sign-in", "allvee", loginScreenController.profileSO.countryCode + loginScreenController.profileSO.mobileNumber, otp, OnSuccessCallback, OnFailedCallback);
        }
    }


    public void OnSuccessCallback(MkiddOOnVerificationSuccessModel result)
    {
        if (result.status_code == 200)
        {
            resultText.text = "Success";
            resultText.color = successColor;
            resultText.gameObject.SetActive(true);
            resultText.gameObject.transform.DOScale(Vector3.one, 0.2f).SetAutoKill(true);
            FileProcessor fileProcessor = new();
            fileProcessor.SaveJsonToFile(fileProcessor.userFilePath, JsonUtility.ToJson(result));
            PlayerPrefs.SetString("access_token", result.accessToken);
            PlayerPrefs.Save();
            Next.gameObject.SetActive(true);
        }
        else
        {
            resultText.text = "Invalid OTP, Try Again";
            resultText.color = failedColor;
            resultText.gameObject.SetActive(true);
            resultText.gameObject.transform.DOScale(Vector3.one, 0.2f).SetAutoKill(true);
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
            string otp = "";
            foreach (var text in PinFields)
            {
                otp += text.text;
            }
            myWebRequest.VerifyOTP("/api/V3/auth/sign-in", "allvee", loginScreenController.profileSO.countryCode + loginScreenController.profileSO.mobileNumber, otp, OnSuccessCallback, OnFailedCallback);
        }
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
        myWebRequest.SendOTP("/api/v2/send-otp", loginScreenController.profileSO.countryCode + loginScreenController.profileSO.mobileNumber, null, null);
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
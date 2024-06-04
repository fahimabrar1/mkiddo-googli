using UnityEngine;
using TMPro;
using System.Diagnostics.Tracing;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
public class LoginPanelTwo : LoginPanelBase
{
    public TMP_Text headerText;
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
            loginScreenController.OnToggleNextButton(false);

    }


    void Start()
    {
        headerText.text = $"Enter your four digit code that we have sent to your mobile number ({loginScreenController.profileSO.countryCode} {loginScreenController.profileSO.mobileNumber})";

        // Ensure all input fields are cleared initially
        foreach (var inputField in PinFields)
        {
            inputField.text = "";
        }

        // Assign the resend button click event
        resendButton.onClick.AddListener(OnClickResendCode);
        myWebRequest.SendOTP("/api/v2/send-otp", loginScreenController.profileSO.countryCode + loginScreenController.profileSO.mobileNumber);
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


    public void OnSuccessCallback()
    {
        loginScreenController.OnToggleNextButton(true);
    }
    public void OnFailedCallback()
    {
        //Todo: error
    }


    public void OnInputValueChange(string val)
    {
        // Update PinFields
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
    }

    public void ClearAllFields()
    {
        foreach (var inputField in PinFields)
        {
            inputField.text = "";
        }

        // Reset the index and select the first input field
        currentIndex = 0;
        if (PinFields.Count > 0)
        {
            // PinFields[0].Select();
            // PinFields[0].ActivateInputField();
        }
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
        myWebRequest.VerifyOTP("/api/V3/auth/sign-in", "allvee", loginScreenController.profileSO.countryCode + loginScreenController.profileSO.mobileNumber, otp, OnSuccessCallback, OnFailedCallback);
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
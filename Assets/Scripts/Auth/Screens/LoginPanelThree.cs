using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginPanelThree : LoginPanelBase
{
    public TMP_Text headerText;
    public TMP_Text resendCodeText;
    public List<TMP_InputField> inputFields;
    private int currentIndex = 0;
    public Button resendButton;
    public Color greyColor;
    public int resendCooldown = 60; // Time in seconds to wait before enabling the resend button



    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        resendCodeText.text = "Resend";
        resendButton.interactable = true;
        resendCodeText.color = Color.black;
    }


    void Start()
    {
        headerText.text = $"Enter your four digit code that we have sent to your mobile number ({loginScreenController.profileSO.countryCode} {loginScreenController.profileSO.mobileNumber})";

        // Ensure all input fields are cleared initially
        foreach (var inputField in inputFields)
        {
            inputField.text = "";
        }

        // Assign the resend button click event
        resendButton.onClick.AddListener(OnClickResendCode);
    }

    public void InputButton(int value)
    {
        if (currentIndex >= inputFields.Count)
        {
            return; // All fields are filled
        }

        // Update the current input field with the value
        inputFields[currentIndex].text = value.ToString();

        // Move to the next input field
        currentIndex++;
        if (currentIndex < inputFields.Count)
        {
            inputFields[currentIndex].Select();
            inputFields[currentIndex].ActivateInputField();
        }
    }

    public void ClearAllFields()
    {
        foreach (var inputField in inputFields)
        {
            inputField.text = "";
        }

        // Reset the index and select the first input field
        currentIndex = 0;
        if (inputFields.Count > 0)
        {
            inputFields[0].Select();
            inputFields[0].ActivateInputField();
        }
    }

    public void OnClickResendCode()
    {
        if (!resendButton.interactable)
        {
            return; // If the button is already disabled, do nothing
        }

        resendCodeText.text = "Code resent!";
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

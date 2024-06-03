using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Diagnostics;
public class LoginPanelOne : LoginPanelBase
{

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

    private int counter = 0;



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
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

        if (numberText.text.Length < 10)
        {
            numberText.text += num.ToString();
            loginScreenController.profileSO.mobileNumber = numberText.text;
            counter++;
            textLimitText.text = counter + "/10";
        }
    }

    public void OnTapNumberPanel()
    {
        panels[0].SetActive(false);
        panels[1].SetActive(true);
    }


    public void OnTapDialerBack()
    {
        counter = 0;
        loginScreenController.profileSO.mobileNumber = "";
        textLimitText.text = "0/10";
        numberText.text = "";
    }


    public void OnUpdateCountryCode(int value)
    {
        var str = Utility.CountryDetails.countryMobileCodes[value];
        var values = str.Split(' ');
        loginScreenController.profileSO.countryCode = values[1];
    }
}
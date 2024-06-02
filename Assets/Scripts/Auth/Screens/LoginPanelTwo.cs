using UnityEngine;
using TMPro;
using System.Diagnostics.Tracing;
using System.Collections.Generic;
public class LoginPanelTwo : LoginPanelBase
{
    public TMP_Dropdown numberDropdown;
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

    }


    public void OnTapDialerButton(int num)
    {
        if (numberText.text.Length < 10)
        {
            numberText.text += num.ToString();
            counter++;
            textLimitText.text = counter + "/10";
        }
    }

    public void OnTapDialerBack()
    {
        counter = 0;
        textLimitText.text = "0/10";
        numberText.text = "";
    }

}
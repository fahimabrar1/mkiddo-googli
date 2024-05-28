using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Diagnostics;
public class LoginPanelOne : LoginPanelBase
{
    public TMP_Dropdown numberDropdown;
    public int numberDropdownIndex = 0;
    public Button TypeHereButton;
    public Button SignInWithGoogle;


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


}
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;

public class LoginPanelThree : LoginPanelBase
{
    public Button Continue;

    [Header("Panel One")]
    public Button ProfilePictureButton;
    public Image ProfileImage;
    public GameObject humanImage;
    public TMP_InputField nameField;

    public TMP_Dropdown DayDropdown;
    public TMP_Dropdown MonthDropdown;
    public TMP_Dropdown YearDropdown;

    void Start()
    {
        InitializeYearDropdown();
        DayDropdown.onValueChanged.AddListener(delegate { UpdateData(); });
        MonthDropdown.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });
        YearDropdown.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });
        Continue.interactable = false;
        UpdateDaysDropdown();
    }


    public void OnChangeName(string val)
    {
        loginScreenController.profileSO.childName = val;
        AllVerification();
    }

    void InitializeYearDropdown()
    {
        List<string> years = new();
        int currentYear = DateTime.Now.Year;
        years.Add("Year");

        for (int i = currentYear; i >= currentYear - 50; i--)
        {
            years.Add(i.ToString());
        }

        YearDropdown.ClearOptions();
        YearDropdown.AddOptions(years);
    }

    void UpdateDaysDropdown()
    {
        int selectedYearIndex = YearDropdown.value;

        int selectedMonthIndex = MonthDropdown.value;
        if (selectedMonthIndex == 0 || selectedYearIndex == 0) return;

        int selectedMonth = int.Parse(MonthDropdown.options[selectedMonthIndex].text);
        int selectedYear = int.Parse(YearDropdown.options[YearDropdown.value].text);

        loginScreenController.profileSO.month = selectedMonth.ToString();
        loginScreenController.profileSO.year = selectedYear.ToString();

        int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);

        List<string> days = new()
        {
            "Day"
        };
        for (int i = 1; i <= daysInMonth; i++)
        {
            days.Add(i.ToString());
        }

        DayDropdown.ClearOptions();
        DayDropdown.AddOptions(days);
        AllVerification();
    }


    public void OnUpdateName(string val)
    {
        nameField.text = val;
        OnChangeName(val);
        AllVerification();
    }



    private void UpdateData()
    {
        int selectedDayIndex = DayDropdown.value;

        if (selectedDayIndex == 0) return;

        int selectedDay = int.Parse(DayDropdown.options[selectedDayIndex].text);
        loginScreenController.profileSO.day = selectedDay.ToString();
        AllVerification();
    }
    public void OnUpdateDay()
    {

    }
    public void OnUpdateMonth()
    {

    }
    public void OnUpdateYear()
    {

    }


    public void OnSelectAvatar(int index)
    {
        loginScreenController.profileSO.avatarPath = index.ToString();
    }



    public void AllVerification()
    {
        Continue.interactable = loginScreenController.profileSO.avatarPath != null && loginScreenController.profileSO.childName != null && loginScreenController.profileSO.day != null && loginScreenController.profileSO.month != null && loginScreenController.profileSO.year != null;
    }



    public void PickImageFromGallery()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Path points to a valid image file
                StartCoroutine(LoadImage(path));
            }
        });

        Debug.Log("Permission result: " + permission);
    }



    private IEnumerator LoadImage(string path)
    {
        string url = "file://" + path;

        using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            if (texture != null)
            {
                // Create a sprite from the texture
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                Sprite sprite = Sprite.Create(texture, rect, pivot);

                // Display the sprite in a UI Image and set to fill the avatar rect
                ProfileImage.sprite = sprite;
                ProfileImage.type = Image.Type.Simple;
                ProfileImage.preserveAspect = false;
                ProfileImage.SetNativeSize();
                // Save the texture to the app's directory
                SaveTextureToFile(texture, "profile_picture.png");
            }
            else
            {
                Debug.LogError("Failed to load texture from " + path);
            }
        }
        else
        {
            Debug.LogError("UnityWebRequest error: " + uwr.error);
        }
    }


    private void SaveTextureToFile(Texture2D texture, string fileName)
    {
        // Encode texture to PNG
        byte[] bytes = texture.EncodeToPNG();

        // Get the path to save the file
        string path = Path.Combine(Application.persistentDataPath, fileName);

        // Save the encoded PNG to the specified path
        File.WriteAllBytes(path, bytes);

        Debug.Log("Saved image to: " + path);
    }
}

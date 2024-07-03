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

    void OnEnable()
    {


        InitializeYearDropdown();
        DayDropdown.onValueChanged.AddListener(delegate { UpdateData(); });
        MonthDropdown.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });
        YearDropdown.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });

        if (loginScreenController.profileSO.id != -1)
        {

            MyDebug.Log($"Name: {loginScreenController.profileSO.childName}");
            MyDebug.Log($"Day: {loginScreenController.profileSO.day}");
            MyDebug.Log($"Month: {loginScreenController.profileSO.month}");
            MyDebug.Log($"Year: {loginScreenController.profileSO.year}");
            MyDebug.Log($"Avatar Path: {loginScreenController.profileSO.avatarPath}");
            nameField.text = loginScreenController.profileSO.childName;
            Continue.interactable = true;
            StartCoroutine(LoadImageFromUrl(loginScreenController.profileSO.avatarPath));
        }
        else
        {
            Continue.interactable = false;
        }

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
        int yearVal = -1;
        if (loginScreenController.profileSO.id != -1)
        {
            int val = int.Parse(loginScreenController.profileSO.year);
            yearVal = currentYear - val + 1;
        }


        for (int i = currentYear; i >= currentYear - 50; i--)
        {
            years.Add(i.ToString());
        }

        YearDropdown.ClearOptions();
        YearDropdown.AddOptions(years);
        YearDropdown.value = yearVal;
    }

    void UpdateDaysDropdown()
    {
        int dayVal = -1;
        int monthVal = -1;
        if (loginScreenController.profileSO.id != -1)
        {
            dayVal = int.Parse(loginScreenController.profileSO.day);
            monthVal = int.Parse(loginScreenController.profileSO.month);
        }


        int selectedYearIndex = YearDropdown.value;

        int selectedMonthIndex = monthVal == -1 ? MonthDropdown.value : monthVal;
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
        DayDropdown.value = dayVal;
        MonthDropdown.value = monthVal;
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
        Continue.interactable = loginScreenController.profileSO.avatarPath.Length > 0 && loginScreenController.profileSO.childName.Length > 0 && loginScreenController.profileSO.day.Length > 0 && loginScreenController.profileSO.month.Length > 0 && loginScreenController.profileSO.year.Length > 0;
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
                loginScreenController.profileSO.childImageSprite = sprite;
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



    private IEnumerator LoadImageFromUrl(string url)
    {
        using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            if (texture != null)
            {
                // Create a sprite from the texture
                Rect rect = new(0, 0, 240, 240);
                Vector2 pivot = new(0.5f, 0.5f);
                Sprite sprite = Sprite.Create(texture, rect, pivot);

                // Display the sprite in a UI Image and set to fill the avatar rect
                ProfileImage.sprite = sprite;
                ProfileImage.type = Image.Type.Simple;
                ProfileImage.preserveAspect = false;
                loginScreenController.profileSO.childImageSprite = sprite;
                humanImage.SetActive(false);
            }
            else
            {
                Debug.LogError("Failed to load texture from " + url);
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

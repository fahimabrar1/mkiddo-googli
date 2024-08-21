using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Collections;


public class LoginPanelThree : LoginPanelBase
{
    public Button Back;
    public Button Next;
    public List<GameObject> panels;

    [Header("Panel One")]
    public Button ProfilePictureButton;
    public Image ProfileImage;
    public GameObject humanImage;

    public TMP_InputField nameField;

    public TMP_Dropdown DayDropdown;
    public TMP_Dropdown MonthDropdown;
    public TMP_Dropdown YearDropdown;

    [Header("Panel Two")]
    public Button ProfilePictureButton1;
    public Image ProfileImage1;
    public TMP_InputField nameField1;
    public TMP_Dropdown DayDropdown1;
    public TMP_Dropdown MonthDropdown1;
    public TMP_Dropdown YearDropdown1;
    public int panel = 0;


    public List<Sprite> avatars;


    void OnEnable()
    {
        panel = 0;
        panels[0].SetActive(true);
        panels[1].SetActive(false);
        InitializeYearDropdown();
        DayDropdown.onValueChanged.AddListener(UpdateDate);
        MonthDropdown.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });
        YearDropdown.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });
        DayDropdown1.onValueChanged.AddListener(UpdateDate);
        MonthDropdown1.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });
        YearDropdown1.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });

        UpdateDaysDropdown();

        ProfilePictureButton1.onClick.AddListener(delegate { PickImageFromGallery(); });
        // Ensure the image fits within the 266x266 size constraint
        loginScreenController.FitImageWithinBounds(ProfileImage, 266, 266);
        loginScreenController.FitImageWithinBounds(ProfileImage1, 266, 266);
    }

    private void UpdateAllDropDowns()
    {
        if (loginScreenController.profileSO.day.Length > 0)
        {
            var val = int.Parse(loginScreenController.profileSO.day);
            DayDropdown.value = val;
            DayDropdown1.value = val;
        }
        if (loginScreenController.profileSO.month.Length > 0)
        {
            var val = int.Parse(loginScreenController.profileSO.month);
            MonthDropdown.value = val;
            MonthDropdown1.value = val;
        }
        if (loginScreenController.profileSO.year.Length > 0)
        {
            var val = DateTime.Now.Year - int.Parse(loginScreenController.profileSO.year) + 1;
            YearDropdown.value = val;
            YearDropdown1.value = val;
        }
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
        YearDropdown1.ClearOptions();
        YearDropdown.AddOptions(years);
        YearDropdown1.AddOptions(years);


        if (loginScreenController.profileSO.month != null || loginScreenController.profileSO.year != null)
        {
            UpdateAllDropDowns();
        }
    }

    void UpdateDaysDropdown()
    {
        int selectedYearIndex = (panel == 0) ? YearDropdown.value : YearDropdown1.value;

        int selectedMonthIndex = (panel == 0) ? MonthDropdown.value : MonthDropdown1.value;

        OnUpdateMonth(selectedMonthIndex);
        OnUpdateYear(selectedYearIndex);

        if (selectedMonthIndex == 0 || selectedYearIndex == 0) return;

        int selectedMonth = int.Parse((panel == 0) ? MonthDropdown.options[selectedMonthIndex].text : MonthDropdown1.options[selectedMonthIndex].text);
        int selectedYear = int.Parse((panel == 0) ? YearDropdown.options[YearDropdown.value].text : YearDropdown1.options[YearDropdown1.value].text);


        OnUpdateMonth(selectedMonthIndex);
        OnUpdateYear(selectedYearIndex);
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
        DayDropdown1.ClearOptions();
        DayDropdown.AddOptions(days);
        DayDropdown1.AddOptions(days);
        AllVerification();
    }



    public void OnTapProfilePanel()
    {
        // DayDropdown.onValueChanged.RemoveAllListeners();
        // MonthDropdown.onValueChanged.RemoveAllListeners();
        // YearDropdown.onValueChanged.RemoveAllListeners();
        panels[0].SetActive(false);
        panels[1].SetActive(true);
        panel++;

    }

    public void OnUpdateName(string val)
    {
        nameField.text = val;
        nameField1.text = val;
        OnChangeName(val);
        AllVerification();
    }



    private void UpdateDate(int index)
    {
        int selectedDayIndex = (panel == 0) ? DayDropdown.value : DayDropdown1.value;

        if (selectedDayIndex == 0) return;

        int selectedDay = int.Parse((panel == 0) ? DayDropdown.options[selectedDayIndex].text : DayDropdown1.options[selectedDayIndex].text);
        loginScreenController.profileSO.day = selectedDay.ToString();
        OnUpdateDay(selectedDayIndex);
        AllVerification();
    }


    public void OnUpdateDay(int selectedIndex)
    {
        MyDebug.Log($"Updating  Date of index:: {selectedIndex}");
        DayDropdown.value = selectedIndex;
        DayDropdown1.value = selectedIndex;
    }

    public void OnUpdateMonth(int selectedIndex)
    {
        MyDebug.Log($"Updating  Month of index:: {selectedIndex}");
        MonthDropdown.value = selectedIndex;
        MonthDropdown1.value = selectedIndex;
    }

    public void OnUpdateYear(int selectedIndex)
    {
        MyDebug.Log($"Updating  Year of index:: {selectedIndex}");
        YearDropdown.value = selectedIndex;
        YearDropdown1.value = selectedIndex;
    }

    public void OnSelectAvatar(int index)
    {
        ProfileImage.sprite = avatars[index];
        ProfileImage1.sprite = avatars[index];

        // Ensure the image fits within the 266x266 size constraint
        loginScreenController.FitImageWithinBounds(ProfileImage, 266, 266);
        loginScreenController.FitImageWithinBounds(ProfileImage1, 266, 266);

        loginScreenController.profileSO.childImageSprite = avatars[index];
        AllVerification();
    }



    // public void AllVerification()
    // {
    //     Next.gameObject.SetActive(loginScreenController.profileSO.avatarIndex != null && loginScreenController.profileSO.childName != null && loginScreenController.profileSO.day != null && loginScreenController.profileSO.month != null && loginScreenController.profileSO.year != null);
    // }


    public void AllVerification()
    {
        Next.gameObject.SetActive(loginScreenController.profileSO.avatarPath.Length > 0 &&
        loginScreenController.profileSO.childName.Length > 0 &&
        loginScreenController.profileSO.day.Length > 0 &&
        loginScreenController.profileSO.month.Length > 0 &&
        loginScreenController.profileSO.year.Length > 0);
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
                Rect rect = new(0, 0, texture.width, texture.height);
                Vector2 pivot = new(0.5f, 0.5f);
                Sprite sprite = Sprite.Create(texture, rect, pivot);

                // Display the sprite in a UI Image and set to fill the avatar rect
                ProfileImage.sprite = sprite;
                ProfileImage.type = Image.Type.Simple;
                ProfileImage.SetNativeSize();

                ProfileImage1.sprite = sprite;
                ProfileImage1.type = Image.Type.Simple;
                ProfileImage1.SetNativeSize();

                // Ensure the image fits within the 266x266 size constraint
                loginScreenController.FitImageWithinBounds(ProfileImage, 266, 266);
                loginScreenController.FitImageWithinBounds(ProfileImage1, 266, 266);

                loginScreenController.profileSO.childImageSprite = sprite;
                loginScreenController.profileSO.avatarPath = url;
                // Save the texture to the app's directory
                SaveTextureToFile(texture, "profile_picture.png");

                AllVerification();
            }
            else
            {
                MyDebug.LogError("Failed to load texture from " + path);
            }
        }
        else
        {
            MyDebug.LogError("UnityWebRequest error: " + uwr.error);
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
                ProfileImage.SetNativeSize();

                ProfileImage1.sprite = sprite;
                ProfileImage1.type = Image.Type.Simple;
                ProfileImage1.SetNativeSize();

                // Ensure the image fits within the 266x266 size constraint
                loginScreenController.FitImageWithinBounds(ProfileImage, 266, 266);
                loginScreenController.FitImageWithinBounds(ProfileImage1, 266, 266);

                loginScreenController.profileSO.childImageSprite = sprite;
                loginScreenController.profileSO.avatarPath = url;
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
        loginScreenController.profileSO.avatarPath = path;
    }
}
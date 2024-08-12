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


    void Start()
    {
        InitializeYearDropdown();
        DayDropdown.onValueChanged.AddListener(delegate { UpdateData(); });
        DayDropdown1.onValueChanged.AddListener(delegate { UpdateData(); });
        MonthDropdown.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });
        YearDropdown.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });
        MonthDropdown1.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });
        YearDropdown1.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });
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
        YearDropdown1.ClearOptions();
        YearDropdown.AddOptions(years);
        YearDropdown1.AddOptions(years);
    }

    void UpdateDaysDropdown()
    {
        int selectedYearIndex = (panel == 0) ? YearDropdown.value : YearDropdown1.value;

        int selectedMonthIndex = (panel == 0) ? MonthDropdown.value : MonthDropdown1.value;
        if (selectedMonthIndex == 0 || selectedYearIndex == 0) return;

        int selectedMonth = int.Parse((panel == 0) ? MonthDropdown.options[selectedMonthIndex].text : MonthDropdown1.options[selectedMonthIndex].text);
        int selectedYear = int.Parse((panel == 0) ? YearDropdown.options[YearDropdown.value].text : YearDropdown1.options[YearDropdown1.value].text);

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
        panels[0].SetActive(false);
        panels[1].SetActive(true);
    }

    public void OnUpdateName(string val)
    {
        nameField.text = val;
        nameField1.text = val;
        OnChangeName(val);
        AllVerification();
    }



    private void UpdateData()
    {
        int selectedDayIndex = (panel == 0) ? DayDropdown.value : DayDropdown1.value;

        if (selectedDayIndex == 0) return;

        int selectedDay = int.Parse((panel == 0) ? DayDropdown.options[selectedDayIndex].text : DayDropdown.options[selectedDayIndex].text);
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
        ProfileImage.sprite = avatars[index];
        ProfileImage1.sprite = avatars[index];
        // loginScreenController.profileSO.avatarIndex = index.ToString();
    }



    // public void AllVerification()
    // {
    //     Next.gameObject.SetActive(loginScreenController.profileSO.avatarIndex != null && loginScreenController.profileSO.childName != null && loginScreenController.profileSO.day != null && loginScreenController.profileSO.month != null && loginScreenController.profileSO.year != null);
    // }


    public void AllVerification()
    {
        Next.interactable = loginScreenController.profileSO.avatarPath.Length > 0 && loginScreenController.profileSO.childName.Length > 0 && loginScreenController.profileSO.day.Length > 0 && loginScreenController.profileSO.month.Length > 0 && loginScreenController.profileSO.year.Length > 0;
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

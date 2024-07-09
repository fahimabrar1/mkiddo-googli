using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine;
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

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        Back.gameObject.SetActive(true);
    }


    void Start()
    {
        InitializeYearDropdown();
        DayDropdown.onValueChanged.AddListener(delegate { UpdateData(); });
        MonthDropdown.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });
        YearDropdown.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });

        YearDropdown.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });
        MonthDropdown1.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });
        YearDropdown1.onValueChanged.AddListener(delegate { UpdateDaysDropdown(); });

        if (loginScreenController.profileSO.id != -1)
        {

            MyDebug.Log($"Name: {loginScreenController.profileSO.childName}");
            MyDebug.Log($"Day: {loginScreenController.profileSO.day}");
            MyDebug.Log($"Month: {loginScreenController.profileSO.month}");
            MyDebug.Log($"Year: {loginScreenController.profileSO.year}");
            MyDebug.Log($"Avatar Path: {loginScreenController.profileSO.avatarPath}");
            nameField.text = loginScreenController.profileSO.childName;
            Next.gameObject.SetActive(true);
            StartCoroutine(LoadImageFromUrl(loginScreenController.profileSO.avatarPath));
        }
        else
        {
            Next.gameObject.SetActive(false);
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
        int dayVal = -1;
        int monthVal = -1;
        if (loginScreenController.profileSO.id != -1)
        {
            dayVal = int.Parse(loginScreenController.profileSO.day);
            monthVal = int.Parse(loginScreenController.profileSO.month);
        }


        int selectedYearIndex = (panel == 0) ? YearDropdown.value : YearDropdown1.value;


        int selectedMonthIndex = monthVal == -1 ? (panel == 0) ? MonthDropdown.value : MonthDropdown1.value : monthVal;

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

        DayDropdown.value = dayVal;
        DayDropdown1.value = dayVal;

        MonthDropdown.value = monthVal;
        MonthDropdown1.value = monthVal;

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
        loginScreenController.profileSO.avatarPath = index.ToString();
    }



    public void AllVerification()
    {
        Next.gameObject.SetActive(loginScreenController.profileSO.avatarPath != null && loginScreenController.profileSO.childName != null && loginScreenController.profileSO.day != null && loginScreenController.profileSO.month != null && loginScreenController.profileSO.year != null);
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
                ProfileImage1.sprite = sprite;
                ProfileImage.type = Image.Type.Simple;
                ProfileImage1.type = Image.Type.Simple;
                ProfileImage.preserveAspect = false;
                ProfileImage1.preserveAspect = false;
                loginScreenController.profileSO.childImageSprite = sprite;
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
}

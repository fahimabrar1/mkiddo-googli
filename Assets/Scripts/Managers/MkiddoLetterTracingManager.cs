using System;
using System.Collections.Generic;
using IndieStudio.EnglishTracingBook.Game;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading.Tasks;

public class MkiddoLetterTracingManager : LevelBaseManager
{


    // Reference to the PanelDataSO scriptable object
    public PanelDataSO panelDataSO;


    public UIManager uIManager;

    public GameObject scrollViewContent;
    public GameObject buttonPrefab;
    public GameObject LockObject;
    public Timer timer;
    public List<LetterHeaderButton> headerButtons;
    public LetterTracingAudioManager letterTracingAudioManager;

    public IndieStudio.EnglishTracingBook.Game.GameManager gameManager;
    public Action<int> OnUpdateButtonUIAction;
    public Action<int> OnTapButtonAction;


    int tempLevel = 0;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void OnEnable()
    {
        level = PlayerPrefs.GetInt($"{panelDataSO.gameName}", 0);
        tempLevel = PlayerPrefs.GetInt($"{panelDataSO.gameName}_temp", 0);

        LockShapeToggle(tempLevel > level);

        ShapesManager.Shape.selectedShapeID = tempLevel;
        AddButtons();
        StarCounts = 3;
    }




    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        foreach (var button in headerButtons)
        {
            OnUpdateButtonUIAction -= button.OnUpdateButtonUI;
        }
    }



    public void AddButtons()
    {
        if (panelDataSO.gamePanelData.blockID == 96)
        {
            if (panelDataSO.gameName == "bangla_shorborno")
            {
                // it's bangla learn_bangla_soroborno
                if (AlphabetLists.AlphabetTypes.TryGetValue("learn_bangla_soroborno", out List<string> sorbornos))
                {
                    CreateHeaderbutton(sorbornos);
                }
            }
            else if (panelDataSO.gameName == "capital_letter_a_to_z")
            {
                // it's english uppercase  english_capital_letters
                if (AlphabetLists.AlphabetTypes.TryGetValue("english_capital_letters", out List<string> sorbornos))
                {
                    CreateHeaderbutton(sorbornos);
                }
            }

        }
        else
        {

            if (panelDataSO.gameName == "english_number_tracing")
            {
                // it's english uppercase  english_capital_letters
                if (AlphabetLists.AlphabetTypes.TryGetValue("english_number", out List<string> sorbornos))
                {
                    CreateHeaderbutton(sorbornos);
                }
            }

        }

        OnUpdateButtonUIAction?.Invoke(tempLevel);

    }



    private void CreateHeaderbutton(List<string> letters)
    {
        headerButtons = new();
        for (int i = 0; i < letters.Count; i++)
        {
            GameObject obj = Instantiate(buttonPrefab, scrollViewContent.transform);
            if (obj.TryGetComponent(out LetterHeaderButton button))
            {
                headerButtons.Add(button);
                button.mkiddoLetterTracingManager = this;
                button.letterText.text = letters[i];
                if (level > i)
                    button.SetCompletedBackground();
                OnUpdateButtonUIAction += button.OnUpdateButtonUI;
                OnTapButtonAction += button.OnTapButton;
                button.ButtonID = i;
            }
        }
    }
    public override void SaveLevel()
    {
        if (tempLevel == level)
            level++;
        PlayerPrefs.SetInt($"{panelDataSO.gameName}", (level == headerButtons.Count - 1) ? 0 : level);
        PlayerPrefs.SetInt($"{panelDataSO.gameName}_temp", level);

        PlayerPrefs.Save();

    }

    internal void SaveTempLevel(int id)
    {
        PlayerPrefs.SetInt($"{panelDataSO.gameName}_temp", id);
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }



    private void LockShapeToggle(bool toggle)
    {
        if (toggle)
        {
            gameManager.DisableGameManager();
            gameManager.DisableHandTracing();
            timer.Stop();
        }

        LockObject.SetActive(toggle);
    }

    internal bool GetGameEnable()
    {
        return tempLevel > level;
    }

    internal void PlayCompleteSfx()
    {
        letterTracingAudioManager.PlayOnSuccessClip();

    }
}

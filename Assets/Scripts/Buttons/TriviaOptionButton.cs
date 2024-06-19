using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TriviaOptionButton : MonoBehaviour
{
    public int id;
    public string prefix;
    public string answer;
    public TMP_Text option;
    public Button button;

    public Image background;
    public Sprite defaultBG;
    public Color whiteColor;

    public Sprite wrongBG;
    public Color wrongColor;
    public Sprite correctBG;
    public Color correctColor;

    public TriviaButtonSelector triviaButtonSelector;

    public void OnResetButton()
    {

        option.color = whiteColor;
        background.sprite = defaultBG;
        button.interactable = true;
    }

    public void OnDisableButton()
    {
        button.interactable = false;
    }


    public void OnCheckAnswer()
    {
        triviaButtonSelector.OnDisableAllButtonAction?.Invoke();
        if ($"{prefix} {option.text}" == answer)
        {
            option.color = correctColor;
            background.sprite = correctBG;
            triviaButtonSelector.OnGameOver();
        }
        else
        {
            option.color = wrongColor;
            background.sprite = wrongBG;
            ResetButtons();
        }
    }


    public async void ResetButtons()
    {
        await Task.Delay(1500);
        triviaButtonSelector.OnResetButtonAction?.Invoke();
    }
}
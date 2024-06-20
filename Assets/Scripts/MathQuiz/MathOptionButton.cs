using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MathOptionButton : MonoBehaviour
{
    public int id;
    public string answer;
    public Image background;
    public TMP_Text option;
    public Button button;
    public MathQuizButtonSelector mathQuizButtonSelector;



    public void OnResetButton()
    {
        button.interactable = true;
        option.enabled = true;
        background.enabled = true;
    }

    public void OnDisableButton()
    {
        button.interactable = false;
    }


    public void OnCheckAnswer()
    {
        mathQuizButtonSelector.OnDisableAllButtonAction?.Invoke();
        if (option.text == answer)
        {
            mathQuizButtonSelector.OnGameOver(option.text);
        }
        else
        {
            mathQuizButtonSelector.OnTapWrongAnswer(option.text);
            ResetButtons();
        }
        option.enabled = false;
        background.enabled = false;
    }


    public async void ResetButtons()
    {
        await Task.Delay(1500);
        mathQuizButtonSelector.OnResetButtonAction?.Invoke();
    }
}

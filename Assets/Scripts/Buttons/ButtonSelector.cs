
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelector : MonoBehaviour

{
    public WorldPuzzleManager worldPuzzleManager;
    public List<GameObject> buttonPrefabs;
    public List<LetterButton> letterButtons;


    private int sphereRadius = 300;
    private string realAnnser;
    private string shuffledAnswer;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        worldPuzzleManager.OnTapLetterButtonAction += OnUpdatedButtons;
        letterButtons = new();
    }

    private void OnUpdatedButtons(int IDByInded)
    {

        MyDebug.Log("On Press Button ID: :" + IDByInded);
        if (letterButtons[IDByInded].isSelected)
        {
            worldPuzzleManager.buttons.Push(letterButtons[IDByInded]);
            worldPuzzleManager.AddLetterContianer(letterButtons[IDByInded]);
        }
        else
        {
            PopByID(IDByInded);
        }
    }

    private void PopByID(int IDByInded)
    {
        if (worldPuzzleManager.buttons.TryPeek(out LetterButton letterButton))
        {

            var letter = worldPuzzleManager.buttons.Pop();
            letter.ResetButton();
            worldPuzzleManager.RemoveContainer(letter.id);
            if (letterButton.id == IDByInded)
            {
                return;
            }
            if (letterButton.id != IDByInded)
            {
                PopByID(IDByInded);
            }


        }
    }

    internal void SetButtons(string question_ans)
    {
        realAnnser = question_ans;
        shuffledAnswer = ShuffleString(question_ans);

        int letterCount = question_ans.Length;

        // Random offset in radians
        float randomOffset = Random.Range(0, Mathf.PI / 2);
        // Create and arrange buttons
        for (int i = 0; i < letterCount; i++)
        {
            // Select a random button prefab
            GameObject randomButtonPrefab = buttonPrefabs[Random.Range(0, buttonPrefabs.Count)];

            // Calculate the position using spherical coordinates
            float theta = i * Mathf.PI * 2 / letterCount + randomOffset; // Angle in radians

            float x = sphereRadius * Mathf.Cos(theta);
            float y = sphereRadius * Mathf.Sin(theta);

            GameObject newButton = Instantiate(randomButtonPrefab, this.transform);
            newButton.transform.localPosition = new Vector3(x, y, 0); // Position on the X-Y plane

            if (newButton.TryGetComponent(out LetterButton letterButton))
            {
                letterButton.id = i;
                letterButton.button.onClick.AddListener(() =>
                {
                    SetLetterButtons(letterButton);
                });
                letterButton.Initialize(shuffledAnswer[i]);
                letterButtons.Add(letterButton);
            }
        }
    }

    private void SetLetterButtons(LetterButton letterButton)
    {
        letterButton.OnClickButton();
        worldPuzzleManager.OnTapLetterButtonAction?.Invoke(letterButton.id);
    }


    private string ShuffleString(string input)
    {
        char[] array = input.ToCharArray();
        System.Random rng = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (array[n], array[k]) = (array[k], array[n]);
        }
        return new string(array);
    }

}

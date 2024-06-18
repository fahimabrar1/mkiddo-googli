
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelector : MonoBehaviour
{
    public List<GameObject> buttonPrefabs;
    public List<LetterButton> letterButtons;


    private int sphereRadius = 300;
    internal void SetButtons(string question_ans)
    {
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
                letterButton.Initialize(question_ans[i]);
                letterButtons.Add(letterButton);
            }
        }
    }

    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

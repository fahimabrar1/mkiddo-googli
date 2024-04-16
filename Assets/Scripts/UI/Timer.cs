using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public float totalTime = 20f; // Total time for the timer
    private float currentTime; // Current time left
    private float previousTime; // Current time left
    public Image progressBar; // Reference to the progress bar image
    public List<Image> stars; // List of Image components representing stars
    public TextMeshProUGUI TimerText; // List of Image components representing stars

    void Start()
    {
        progressBar.fillAmount = 0f;
        currentTime = 0; // Set current time to total time at the start
        previousTime = 0; // Set previous time
        TimerText.text = totalTime.ToString() + "s";
    }

    void Update()
    {
        // Decrease current time
        currentTime += Time.deltaTime;

        // Calculate fill amount for progress bar
        float fillAmount = currentTime / totalTime;
        UpdateTimerText();
        // Update progress bar fill amount
        progressBar.fillAmount = fillAmount;

        // Change star colors at specific intervals
        ChangeStarColors(fillAmount);

        // Check if time is up
        if (currentTime >= totalTime)
        {

            //Todo:Time's up, do something here (e.g., end game, reset timer, etc.)
            currentTime = totalTime; // Reset timer
        }
    }

    private void UpdateTimerText()
    {
        int newPrevTime = Mathf.FloorToInt(previousTime);
        int newCurTime = Mathf.FloorToInt(currentTime);
        if (newPrevTime < newCurTime)
        {
            previousTime = newCurTime;
            TimerText.text = (totalTime - newCurTime).ToString() + "s";
        }
    }

    void ChangeStarColors(float fillAmount)
    {
        // Calculate the index of the stars to change color
        int index1 = Mathf.FloorToInt(stars.Count * 0.25f);
        int index2 = Mathf.FloorToInt(stars.Count * 0.5f);
        int index3 = Mathf.FloorToInt(stars.Count * 0.75f);

        // Change the color of stars at the specified intervals
        if (fillAmount >= 0.75f && index3 >= 0 && index3 < stars.Count)
        {
            stars[index3].color = Color.grey;
        }
        if (fillAmount >= 0.5f && index2 >= 0 && index2 < stars.Count)
        {
            stars[index2].color = Color.grey;
        }
        if (fillAmount >= 0.25f && index1 >= 0 && index1 < stars.Count)
        {
            stars[index1].color = Color.grey;
        }
    }
}

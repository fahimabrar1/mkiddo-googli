using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using System;

public class ProgressBarTimer : MonoBehaviour
{
    public float totalTime = 20f; // Total time for the timer
    private float currentTime; // Current time left
    private float previousTime; // Current time left
    private bool isActive = true; // if the timer is active
    public Image progressBar; // Reference to the progress bar image
    public LevelBaseManager levelBaseManager;
    public List<GameObject> activeStars; // List of Image components representing stars
    public List<GameObject> inactiveStars; // List of Image components representing stars
    public UnityEvent OnTimeUp;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        levelBaseManager = FindObjectOfType<LevelBaseManager>();
    }


    void Start()
    {
        progressBar.fillAmount = 0f;
        currentTime = 0; // Set current time to total time at the start
        previousTime = 0; // Set previous time
        levelBaseManager.StarCounts = 2;
    }

    void Update()
    {
        if (!isActive)
            return;
        // Decrease current time
        currentTime += Time.deltaTime;

        // Calculate fill amount for progress bar
        float fillAmount = 1 - (currentTime / totalTime);
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
            Stop();
            OnTimeUp?.Invoke();

        }
    }

    private void UpdateTimerText()
    {
        int newPrevTime = Mathf.FloorToInt(previousTime);
        int newCurTime = Mathf.FloorToInt(currentTime);
        if (newPrevTime < newCurTime)
        {
            previousTime = newCurTime;
        }
    }
    void ChangeStarColors(float fillAmount)
    {
        // Define the thresholds for star color changes
        float threshold1 = 0.66f;
        float threshold2 = 0.33f;
        if (fillAmount <= 0)
        {

            activeStars[0].SetActive(false);
            inactiveStars[0].SetActive(true);

            levelBaseManager.StarCounts = 0;
        }
        else if (fillAmount <= threshold2)
        {

            activeStars[1].SetActive(false);
            inactiveStars[1].SetActive(true);

            levelBaseManager.StarCounts = 1;
        }
        else if (fillAmount <= threshold1)
        {

            activeStars[2].SetActive(false);
            inactiveStars[2].SetActive(true);

            levelBaseManager.StarCounts = 2;
        }
        else
        {
            levelBaseManager.StarCounts = 3;

        }
    }

    public void Stop()
    {
        isActive = false;
    }
}


[Serializable]
public class ImageListModel
{
    public List<Image> iamges;
}
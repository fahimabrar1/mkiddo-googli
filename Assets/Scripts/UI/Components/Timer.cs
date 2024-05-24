using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float totalTime = 20f; // Total time for the timer
    private float currentTime; // Current time left
    private float previousTime; // Current time left
    private bool isActive; // if the timer is active
    public Image progressBar; // Reference to the progress bar image
    public LevelBaseManager levelBaseManager;
    public List<Image> stars; // List of Image components representing stars
    public TextMeshProUGUI TimerText; // List of Image components representing stars
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
        if (TimerText != null)
            TimerText.text = totalTime.ToString() + "s";
        isActive = true;
    }

    void Update()
    {
        if (!isActive)
            return;
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
            if (TimerText != null)
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

            levelBaseManager.StarCounts = 0;
        }
        else if (fillAmount >= 0.5f && index2 >= 0 && index2 < stars.Count)
        {
            stars[index2].color = Color.grey;
            levelBaseManager.StarCounts = 1;
        }
        else if (fillAmount >= 0.25f && index1 >= 0 && index1 < stars.Count)
        {
            stars[index1].color = Color.grey;
            levelBaseManager.StarCounts = 2;
        }

    }

    public void Stop()
    {
        isActive = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeGridPanelController : MonoBehaviour
{

    public int pageID;
    /// <summary>
    /// below is the folder names, where we will keep the assets
    /// imgsort
    /// dnd
    /// imgmatch
    /// </summary>
    public string gameTypeName;

    /// <summary>
    /// content_category:1 is for image sort
    /// content_category:6 is for image sort
    /// content_category:1 is for mathcing 2 side
    /// </summary>
    public string contentCategory;

    /// <summary>
    /// blockSlug for image sort: SORT_BY_RULE
    /// blockSlug for image sort: DRAG_N_DROP
    /// blockSlug for image sort: TWOSIDEMATCHING
    /// </summary>
    public string contentType;


    public List<HomeGridPanel> homeGridPanels = new();
    public SwipeSlider swipeSlider;
    public GameObject HomePanelPrefab;
    public GameObject PagePrefab;
    public GameObject ProgressBarObject;
    public Slider ProgressBarSlider;
    public Transform ContentParent;
    public List<Transform> Pages;

    private List<float> downloadProgresses;
    private int totalDownloads;
    MyWebRequest myWebRequest;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        swipeSlider.GoToStart();
    }

    // Start is called before the first frame update
    void Start()
    {
        myWebRequest = new();

        StartCoroutine(myWebRequest.FetchData($"/api/v3/content/content-list?content_category={contentCategory}", contentType: contentType, OnApiResponseSucces: OnSuccessLoadingScreen));
    }

    private void OnSuccessLoadingScreen(OnApiResponseSuccess onApiResponseSuccess)
    {
        var res = (onApiResponseSuccess.videoBlock.contents.Count / 6) + 1;
        for (int i = 0; i < res; i++)
        {
            GameObject gameObject = Instantiate(PagePrefab, ContentParent);
            Pages.Add(gameObject.transform);
            swipeSlider.Pages.Add(gameObject);

        }
        swipeSlider.SetButtons();

        foreach (var content in onApiResponseSuccess.videoBlock.contents)
        {
            MyDebug.Log("Content Name: " + content.name);
            MyDebug.Log("Content Link: " + content.link);
            // Access other properties as needed
        }
        Initialize(onApiResponseSuccess.videoBlock.contents);
    }


    public void Initialize(List<Content> content)
    {
        for (int i = 0; i < content.Count; i++)
        {
            GameObject gameObject = Instantiate(HomePanelPrefab, Pages[i / 6]);
            if (gameObject.TryGetComponent(out HomeGridPanel panel))
            {
                homeGridPanels.Add(panel);
                panel.SetContent(content[i], this);
            }
        }
    }


    public void OnDownloadAllContent()
    {
        totalDownloads = homeGridPanels.Count;
        downloadProgresses = new List<float>(new float[totalDownloads]);

        ProgressBarObject.SetActive(true);
        ProgressBarSlider.value = 0;

        for (int i = 0; i < totalDownloads; i++)
        {
            StartCoroutine(myWebRequest.DownloadAndUnzip(homeGridPanels[i].content.link, homeGridPanels[i].GetZipTheFileName(), gameTypeName, i, OnUpdateDownloadProgress));
        }
    }
    private void OnUpdateDownloadProgress(float downloadValue, int downloadId)
    {
        if (downloadProgresses == null)
        {
            Debug.LogError("downloadProgresses list is null.");
            return;
        }

        if (downloadId < 0 || downloadId >= downloadProgresses.Count)
        {
            Debug.LogError($"Download ID {downloadId} is out of range. downloadProgresses.Count: {downloadProgresses.Count}");
            return;
        }
        downloadProgresses[downloadId] = downloadValue;
        UpdateTotalProgress();
    }

    private void UpdateTotalProgress()
    {
        float totalProgress = 0;

        foreach (var progress in downloadProgresses)
        {
            totalProgress += progress;
        }

        totalProgress /= totalDownloads;
        ProgressBarSlider.value = totalProgress;
        if (totalProgress == 1)
        {
            ProgressBarSlider.value = 0;
            ProgressBarObject.SetActive(false);
        }
        // Update your progress bar UI here
        // e.g., progressBar.fillAmount = totalProgress;
        Debug.Log("Total Progress: " + totalProgress);
    }
}

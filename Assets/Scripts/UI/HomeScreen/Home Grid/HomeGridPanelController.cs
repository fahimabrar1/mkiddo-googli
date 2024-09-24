using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HomeGridPanelController : MonoBehaviour
{

    public int pageID;

    public GamePanelData gamePanelData;
    public List<HomeGridPanel> homeGridPanels = new();
    public SwipeSlider swipeSlider;
    public GameObject HomePanelPrefab;
    public GameObject PagePrefab;
    public GameObject ProgressBarObject;
    public Slider ProgressBarSlider;
    public TextMeshProUGUI ProgressBarTexct;
    public Transform ContentParent;
    public List<int> showByContentID;
    public List<Transform> Pages;
    public PanelDataSO PanelDataSO;

    private List<float> downloadProgresses;
    private int totalDownloads;
    MyWebRequest myWebRequest;

    private Action OnPanelPress;
    private bool alreadyFetched = false;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        swipeSlider.GoToStart();
        myWebRequest = new();

        if (!alreadyFetched)
            StartCoroutine(myWebRequest.FetchData($"/api/v3/content/content-list?content_category={gamePanelData.contentCategory}", blockID: gamePanelData.blockID, contentType: gamePanelData.contentType, OnApiResponseSucces: OnSuccessLoadingScreen));
    }



    private void OnSuccessLoadingScreen(OnApiResponseSuccess onApiResponseSuccess)
    {
        alreadyFetched = true;
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
        Initialize(onApiResponseSuccess.videoBlock);
    }


    public void Initialize(VideoBlock videoBlock)
    {
        // Filter contents based on the condition involving showByContentID
        List<Content> contents = new();
        if (showByContentID.Count > 0)
        {
            contents = videoBlock.contents.FindAll(c => showByContentID.Contains(c.content_id));
        }
        else
        {
            contents = videoBlock.contents;
        }

        // Initialize contents in pages
        for (int i = 0; i < contents.Count; i++)
        {
            // Instantiate HomePanelPrefab in the correct page
            GameObject gameObject = Instantiate(HomePanelPrefab, Pages[i / 6]);
            if (gameObject.TryGetComponent(out HomeGridPanel panel))
            {
                var splits = contents[i].link.Split('/');
                var folderName = splits.Last().Split('.');
                homeGridPanels.Add(panel);
                panel.SetContent(i, videoBlock.block_id, contents[i], folderName[0], this);
            }
        }
    }


    public void OnDownloadAllContent(Action OnPanelPress)
    {

        totalDownloads = gamePanelData.contentType.Equals("DRAG_N_DROP") ? homeGridPanels.Count + 1 : homeGridPanels.Count;
        downloadProgresses = new List<float>(new float[totalDownloads]);

        ProgressBarObject.SetActive(true);
        ProgressBarSlider.value = 0;

        for (int i = 0; i < totalDownloads; i++)
        {
            if (gamePanelData.contentType.Equals("DRAG_N_DROP") && i == totalDownloads - 1)
            {
                StartCoroutine(myWebRequest.DownloadAndUnzip("https://s3.mkiddo.com//storage/Interactive_Learning/Drag_and_drop/drag_comb.zip", "drag_comb", gamePanelData.gameTypeName, i, OnUpdateDownloadProgress));
                break;
            }
            StartCoroutine(myWebRequest.DownloadAndUnzip(homeGridPanels[i].content.link, homeGridPanels[i].GetZipTheFileName(), gamePanelData.gameTypeName, i, OnUpdateDownloadProgress));

        }
        this.OnPanelPress = OnPanelPress;
    }
    private void OnUpdateDownloadProgress(float downloadValue, int downloadId)
    {
        if (downloadProgresses == null)
        {
            MyDebug.LogError("downloadProgresses list is null.");
            return;
        }

        if (downloadId < 0 || downloadId >= downloadProgresses.Count)
        {
            MyDebug.LogError($"Download ID {downloadId} is out of range. downloadProgresses.Count: {downloadProgresses.Count}");
            return;
        }
        downloadProgresses[downloadId] = downloadValue;
        StartCoroutine(UpdateTotalProgress());
    }

    private IEnumerator UpdateTotalProgress()
    {
        float totalProgress = 0;

        // Calculate total progress from the download progress list
        foreach (var progress in downloadProgresses)
        {
            totalProgress += progress;
        }

        totalProgress /= totalDownloads;

        // Update progress bar UI
        ProgressBarSlider.value = totalProgress;
        ProgressBarTexct.text = "Downloading: " + Mathf.RoundToInt(totalProgress * 100).ToString() + "%";

        // If download is complete (totalProgress is 100%)
        if (totalProgress == 1)
        {
            foreach (var panel in homeGridPanels)
            {
                panel.SetSaveLevelByuContentFOlderName();
            }

            // Wait for half a second before proceeding
            yield return new WaitForSeconds(0.5f);

            // Trigger the panel press event
            OnPanelPress?.Invoke();
            OnPanelPress = null;

            // Reset progress bar
            ProgressBarSlider.value = 0;
            ProgressBarObject.SetActive(false);
        }

        // Log the progress
        MyDebug.Log("Total Progress: " + totalProgress);
    }
}

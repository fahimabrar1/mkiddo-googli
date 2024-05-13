using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeContentFetcher : MonoBehaviour
{
    MyWebRequest myWebRequest;
    // Start is called before the first frame update
    void Start()
    {
        myWebRequest = new();

        StartCoroutine(myWebRequest.FetchData("/api/v3/content/content-list?content_category=1", blockSlug: "Sort-by-rule", OnApiResponseSucces: OnSuccessLoadingScreen));
    }

    private void OnSuccessLoadingScreen(OnApiResponseSuccess onApiResponseSuccess)
    {
        foreach (var content in onApiResponseSuccess.videoBlock.contents)
        {

            MyDebug.Log("Content Name: " + content.name);
            MyDebug.Log("Content Link: " + content.link);
            // Access other properties as needed

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

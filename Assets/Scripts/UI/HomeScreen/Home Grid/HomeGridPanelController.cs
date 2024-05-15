using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeGridPanelController : MonoBehaviour
{

    public List<HomeGridPanel> homeGridPanels = new();

    public GameObject HomePanelPrefab;
    public GameObject PagePrefab;
    public Transform ContentParent;
    public List<Transform> Pages;
    MyWebRequest myWebRequest;


    // Start is called before the first frame update
    void Start()
    {
        myWebRequest = new();

        StartCoroutine(myWebRequest.FetchData("/api/v3/content/content-list?content_category=1", blockSlug: "Sort-by-rule", OnApiResponseSucces: OnSuccessLoadingScreen));
    }

    private void OnSuccessLoadingScreen(OnApiResponseSuccess onApiResponseSuccess)
    {
        var res = (onApiResponseSuccess.videoBlock.contents.Count / 6) + 1;
        for (int i = 0; i < res; i++)
        {
            GameObject gameObject = Instantiate(PagePrefab, ContentParent);
            Pages.Add(gameObject.transform);
        }

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
                panel.SetContent(content[i]);
            }
        }
    }
}

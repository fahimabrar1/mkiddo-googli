using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using IndieStudio.EnglishTracingBook.Game;

public class HomeGridPanel : MonoBehaviour
{
    public int ID;
    public Image image;
    public Content content;
    public GameObject loadinginddicator;
    public int VideoBlockID;
    public string contentTypeFolderName;
    public string gameName;

    public HomeGridPanelController homeGridPanelController;

    private MyWebRequest myWebRequest;

    private string sceneID = "Home";


    internal void SetContent(int ID, int blockID, Content content, string folderName, HomeGridPanelController homeGridPanelController)
    {
        this.ID = ID;
        this.homeGridPanelController = homeGridPanelController;
        myWebRequest = new MyWebRequest();
        contentTypeFolderName = folderName;
        gameName = content.name.ToSnakeCase();
        this.content = content;
        VideoBlockID = blockID;
        loadinginddicator.SetActive(true);

        StartCoroutine(myWebRequest.FetchImageIEnumeratorWeb(content.thumbnail, image, OnFetchComplete: OnCompleteFetching));
    }

    private void OnCompleteFetching()
    {
        loadinginddicator.SetActive(false);
    }


    public void OnClickPanel()
    {
        // if no directory exits, then the game is not downloaded
        if (!Directory.Exists(Application.persistentDataPath + $"/googli/{homeGridPanelController.gamePanelData.gameTypeName}"))
        {
            homeGridPanelController.OnDownloadAllContent(OnPanelPress: OnLoadGameScene);
        }
        else
        {
            OnLoadGameScene();
        }
    }


    public void OnLoadGameScene()
    {
        SetSceneID();

        homeGridPanelController.PanelDataSO.gamePanelData.blockID = homeGridPanelController.gamePanelData.blockID;
        homeGridPanelController.PanelDataSO.gamePanelData.gameTypeName = homeGridPanelController.gamePanelData.gameTypeName;
        homeGridPanelController.PanelDataSO.gamePanelData.contentCategory = homeGridPanelController.gamePanelData.contentCategory;
        homeGridPanelController.PanelDataSO.gamePanelData.contentType = homeGridPanelController.gamePanelData.contentType;
        homeGridPanelController.PanelDataSO.contentTypeFolderName = contentTypeFolderName;
        homeGridPanelController.PanelDataSO.gameName = content.name.ToSnakeCase();

        SceneManager.LoadSceneAsync(sceneID);
    }


    public string GetZipTheFileName()
    {
        var linkSplit = content.link.Split('/');
        string lastName = linkSplit.Last();
        var names = lastName.Split('.');
        return names.First();

    }


    public void SetSceneID()
    {
        if (VideoBlockID.Equals(117))
        {
            // Image Sorting
            sceneID = "Image Sorting";
        }
        else if (VideoBlockID.Equals(107))

        {
            sceneID = "Drag And Drop";
        }
        else if (VideoBlockID.Equals(137))
        {
            sceneID = "Matching 2 Sides";
        }
        else if (VideoBlockID.Equals(128))
        {
            // Flash Cards
            sceneID = "Flash Cards";
        }
        else if (VideoBlockID.Equals(96))
        {
            // Letter Traching
            sceneID = "Letter Tracing";
            int level = MyPlayerPrefabs.Instance.GetInt($"{content.name.ToSnakeCase()}", 0);
            MyPlayerPrefabs.Instance.SetInt($"{content.name.ToSnakeCase()}_temp", level);
            ShapesManager shapesManager = ShapesManager.shapesManagers["UShapesManager"];
            ShapesManager.shapesManagerReference = "UShapesManager";
        }
        else if (VideoBlockID.Equals(97))
        {
            // Number Traching
            sceneID = "Letter Tracing";
            int level = MyPlayerPrefabs.Instance.GetInt($"{content.name.ToSnakeCase()}", 0);
            MyPlayerPrefabs.Instance.SetInt($"{content.name.ToSnakeCase()}_temp", level);
            ShapesManager shapesManager = ShapesManager.shapesManagers["NShapesManager"];
            ShapesManager.shapesManagerReference = "NShapesManager";
        }
        else
        {
            sceneID = "Home";
        }

    }

    internal void SetSaveLevelByuContentFOlderName()
    {
        MyPlayerPrefabs.Instance.SetInt($"{gameName}", 0);
    }
}


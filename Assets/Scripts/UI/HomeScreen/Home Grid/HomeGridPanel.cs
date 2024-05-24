using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class HomeGridPanel : MonoBehaviour
{
    public int ID;
    public Image image;
    public Content content;
    public string contentTypeFolderName;

    public HomeGridPanelController homeGridPanelController;

    private MyWebRequest myWebRequest;

    private int sceneID;


    internal void SetContent(int ID, Content content, string folderName, HomeGridPanelController homeGridPanelController)
    {
        this.ID = ID;
        this.homeGridPanelController = homeGridPanelController;
        myWebRequest = new MyWebRequest();
        contentTypeFolderName = folderName;
        this.content = content;
        myWebRequest.FetchImageAsync(content.thumbnail, image);
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

        homeGridPanelController.PanelDataSO.gamePanelData.gameTypeName = homeGridPanelController.gamePanelData.gameTypeName;
        homeGridPanelController.PanelDataSO.gamePanelData.contentCategory = homeGridPanelController.gamePanelData.contentCategory;
        homeGridPanelController.PanelDataSO.gamePanelData.contentType = homeGridPanelController.gamePanelData.contentType;
        homeGridPanelController.PanelDataSO.contentTypeFolderName = contentTypeFolderName;

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
        if (content.content_type.Equals("SORT_BY_RULE"))
        {
            sceneID = 1;
        }
        else if (content.content_type.Equals("DRAG_N_DROP"))
        {
            sceneID = 2;
        }
        else
        {
            sceneID = 3;
        }

    }

    internal void SetSaveLevelByuContentFOlderName()
    {
        PlayerPrefs.SetInt($"{contentTypeFolderName}", 0);
        PlayerPrefs.Save();
    }
}


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
    public int VideoBlockID;
    public string contentTypeFolderName;
    public string gameName;

    public HomeGridPanelController homeGridPanelController;

    private MyWebRequest myWebRequest;

    private int sceneID;


    internal void SetContent(int ID, int blockID, Content content, string folderName, HomeGridPanelController homeGridPanelController)
    {
        this.ID = ID;
        this.homeGridPanelController = homeGridPanelController;
        myWebRequest = new MyWebRequest();
        contentTypeFolderName = folderName;
        gameName = content.name.ToSnakeCase();
        this.content = content;
        VideoBlockID = blockID;

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
            sceneID = 2;
        }
        else if (VideoBlockID.Equals(107))

        {
            sceneID = 3;
        }
        else if (VideoBlockID.Equals(137))
        {
            sceneID = 4;
        }
        else if (VideoBlockID.Equals(128))
        {
            sceneID = 5;
        }
        else if (VideoBlockID.Equals(96))
        {
            sceneID = 6;
            ShapesManager shapesManager = ShapesManager.shapesManagers["UShapesManager"];
            ShapesManager.shapesManagerReference = "UShapesManager";
        }

    }

    internal void SetSaveLevelByuContentFOlderName()
    {
        PlayerPrefs.SetInt($"{gameName}", 0);
        PlayerPrefs.Save();
    }
}


using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeGridPanel : MonoBehaviour
{
    public Image image;
    public Content content;
    public string contentTypeFolderName;

    public HomeGridPanelController homeGridPanelController;

    private MyWebRequest myWebRequest;

    private int sceneID;


    internal void SetContent(Content content, string folderName, HomeGridPanelController homeGridPanelController)
    {
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
            if (content.content_type.Equals("SORT_BY_RULE"))
            {
                SetSceneID(1);
            }
            else if (content.content_type.Equals("DRAG_N_DROP"))
            {
                SetSceneID(2);
            }
            else
            {
                SetSceneID(3);
            }
            OnLoadGameScene();
        }
    }


    public void OnLoadGameScene()
    {
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


    public void SetSceneID(int value)
    {
        sceneID = value;
    }
}


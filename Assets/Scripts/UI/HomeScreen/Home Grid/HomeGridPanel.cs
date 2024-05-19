using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HomeGridPanel : MonoBehaviour
{
    public Image image;
    public Content content;

    public HomeGridPanelController homeGridPanelController;


    private MyWebRequest myWebRequest;
    internal void SetContent(Content content, HomeGridPanelController homeGridPanelController)
    {
        this.homeGridPanelController = homeGridPanelController;
        myWebRequest = new MyWebRequest();

        this.content = content;
        myWebRequest.FetchImageAsync(content.thumbnail, image);
    }


    public void OnClickPanel()
    {
        // if no directory exits, then the game is not downloaded
        if (!Directory.Exists(Application.persistentDataPath + $"/googli/{homeGridPanelController.gameTypeName}"))
        {
            homeGridPanelController.OnDownloadAllContent();
        }
    }


    public string GetZipTheFileName()
    {
        var linkSplit = content.link.Split('/');
        string lastName = linkSplit.Last();
        var names = lastName.Split('.');
        return names.First();

    }
}

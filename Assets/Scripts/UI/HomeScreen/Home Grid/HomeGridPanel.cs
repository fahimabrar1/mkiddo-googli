using UnityEngine;
using UnityEngine.UI;

public class HomeGridPanel : MonoBehaviour
{
    public Image image;
    private Content content;

    private MyWebRequest myWebRequest;
    internal void SetContent(Content content)
    {
        myWebRequest = new MyWebRequest();

        this.content = content;
        myWebRequest.FetchImageAsync(content.thumbnail, image);
    }
}

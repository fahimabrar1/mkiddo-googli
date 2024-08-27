using UnityEngine;
using UnityEngine.UI;


public class HomeGridPanelAkibuki : MonoBehaviour
{
    public int ID;
    public bool isOpenCanbas;
    public Image image;
    public Sprite sprite;
    public AkibukiHomeController akibukiHomeController;



    public void OnClickPanel()
    {
        akibukiHomeController.OnClickSubPanel(ID);
    }
}


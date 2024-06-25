using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using IndieStudio.EnglishTracingBook.Game;

public class HomeGridPanelAkibuki : MonoBehaviour
{
    public int ID;
    public bool isOpenCanbas;
    public Image image;
    public Sprite sprite;
    public AkibukiHomeController akibukiHomeController;



    public void OnClickPanel()
    {
        akibukiHomeController.OnClickPanel(ID);
    }
}


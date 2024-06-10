using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HomePageController : MonoBehaviour
{
    public List<GameObject> pages = new();
    public List<Image> ButtomOutlines = new();

    public Action<int> OnUpdatePageAction;

    public void OnClickToChangePage(int pageIndex)
    {

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(false);
            ButtomOutlines[i].enabled = false;

        }
        pages[pageIndex].SetActive(true);
        ButtomOutlines[pageIndex].enabled = true;
    }
}

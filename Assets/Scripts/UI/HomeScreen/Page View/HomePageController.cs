using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePageController : MonoBehaviour
{
    public Image header;
    public List<GameObject> pages = new();
    public List<Image> ButtomOutlines = new();
    public List<int> headerXOffset = new();

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
        // Move the header to the selected button position
        MoveHeaderToButton(pageIndex);
    }


    private void MoveHeaderToButton(int pageIndex)
    {
        header.rectTransform.anchoredPosition = new(headerXOffset[pageIndex], 0);
    }
}

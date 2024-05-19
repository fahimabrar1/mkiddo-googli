using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HomePageController : MonoBehaviour
{
    public List<GameObject> pages = new();

    public Action<int> OnUpdatePageAction;

    public void OnClickToChangePage(int pageIndex)
    {

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(false);
        }

        pages[pageIndex].SetActive(true);
    }
}

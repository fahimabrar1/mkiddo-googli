using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Linq;

public class AkibukiHomeController : MonoBehaviour
{
    public List<HomeGridPanelAkibuki> homeGridPanelAkibukis;


    public List<GameObject> panels;
    public List<GameObject> ContentPanels;

    public GameObject backButton;

    public int panelIndex;
    public bool isOnSubPanels;




    public AkibukiConfigSO akibukiConfigSO;


    public void OnClickSubPanel(int index)
    {
        isOnSubPanels = true;
        OnDisablePanel(panels);
        OnUpdatePanel(ContentPanels, index);
    }



    public void OnClickPanel(int index)
    {
        akibukiConfigSO.panelID = homeGridPanelAkibukis[index].ID;
        akibukiConfigSO.sprite = homeGridPanelAkibukis[index].sprite;
        akibukiConfigSO.isOpenCanbas = index == 0;

        SceneManager.LoadSceneAsync(2);
    }


    public void OnBackPress()
    {
        if (isOnSubPanels)
        {
            OnDisablePanel(ContentPanels);
            isOnSubPanels = false;
        }
        else if (panelIndex > 0)
        {
            OnDecrementPanelIndex();
            backButton.SetActive(panelIndex != 0);
        }
        OnUpdatePanel();
    }



    public void OnDisablePanel(List<GameObject> objects)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(false);
        }
    }
    public void OnUpdatePanel()
    {
        OnDisablePanel(panels);
        panels[panelIndex].SetActive(true);
    }


    public void OnUpdatePanel(List<GameObject> objects, int index)
    {
        OnDisablePanel(objects);

        objects[index].SetActive(true);
    }

    public void OnIncrementPanelIndex()
    {
        panelIndex++;
        backButton.SetActive(true);
    }


    public void OnDecrementPanelIndex()
    {
        panelIndex--;
    }
}

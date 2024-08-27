using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Linq;

public class AkibukiHomeController : MonoBehaviour
{
    public List<HomeGridPanelAkibuki> homeGridPanelAkibukis;


    public List<GameObject> panels;

    public GameObject backButton;

    public int panelIndex;




    public AkibukiConfigSO akibukiConfigSO;


    public void OnClickSubPanel(int index)
    {
        OnIncrementPanelIndex();
        OnUpdatePanel();
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
        if (panelIndex > 0)
        {
            OnDecrementPanelIndex();
            backButton.SetActive(panelIndex != 0);
        }
        OnUpdatePanel();
    }



    public void OnUpdatePanel()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(false);
        }

        panels[panelIndex].SetActive(true);
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

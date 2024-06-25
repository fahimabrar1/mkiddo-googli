using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class AkibukiHomeController : MonoBehaviour
{
    public List<HomeGridPanelAkibuki> homeGridPanelAkibukis;

    public AkibukiConfigSO akibukiConfigSO;


    public void OnClickPanel(int index)
    {
        akibukiConfigSO.panelID = homeGridPanelAkibukis[index].ID;
        akibukiConfigSO.sprite = homeGridPanelAkibukis[index].sprite;
        akibukiConfigSO.isOpenCanbas = index == 0;

        SceneManager.LoadSceneAsync(2);
    }
}

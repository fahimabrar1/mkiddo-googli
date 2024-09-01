using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AkibukiHomeSubController : MonoBehaviour
{

    public List<GameObject> panels;

    public int panelIndex = 0;
    public Button NextButton;
    public Button BackButton;
    public List<Image> Indicators;
    public Color IndicatorColor;
    public Color WhiteColor;


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        NextButton.onClick.AddListener(() => OnIncrementPanelIndex());
        BackButton.onClick.AddListener(() => OnDecrementPanelIndex());

        OnUpdatePanel();
        panelIndex = 0;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        NextButton.onClick.RemoveAllListeners();
        BackButton.onClick.RemoveAllListeners();
    }




    public void OnUpdatePanel()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(false);
        }

        for (int i = 0; i < Indicators.Count; i++)
        {
            Indicators[i].color = WhiteColor;
        }

        panels[panelIndex].SetActive(true);
        Indicators[panelIndex].color = IndicatorColor;
        OnUpdateNavigatingButtons();
    }

    public void OnIncrementPanelIndex()
    {
        panelIndex++;
        OnUpdatePanel();
    }


    public void OnDecrementPanelIndex()
    {
        panelIndex--;
        OnUpdatePanel();
    }


    public void OnUpdateNavigatingButtons()
    {
        BackButton.gameObject.SetActive(panelIndex != 0);
        NextButton.gameObject.SetActive(panelIndex == 0);
    }
}

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



    // Start is called before the first frame update
    void Start()
    {
        NextButton.onClick.AddListener(() => OnIncrementPanelIndex());
        BackButton.onClick.AddListener(() => OnDecrementPanelIndex());

        panelIndex = 0;
        OnUpdatePanel();
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

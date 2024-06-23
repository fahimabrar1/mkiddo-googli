using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    public List<BrushButton> buttons;

    public Action<int> OnBrushSelectAction;

    public AkibukiManager akibukiManager;


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        foreach (var btn in buttons)
        {
            OnBrushSelectAction += btn.OnSetBrush;
        }
    }



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        OnBrushSelect(1);
    }



    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        foreach (var btn in buttons)
        {
            OnBrushSelectAction -= btn.OnSetBrush;
        }
    }


    public void OnBrushSelect(int brush)
    {
        akibukiManager.OnToggleStickeMode(false);
        akibukiManager.OnResetStickers();
        OnBrushSelectAction?.Invoke(brush);
        float width = .4f;
        if (brush == 1)
            akibukiManager.SetLineWidth(.55f);
        else if (brush == 2)
        {
            akibukiManager.SetLineWidth(.75f);
        }
        else
        {
            akibukiManager.SetLineWidth(width);
        }
    }

}

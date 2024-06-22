using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColorLayoutManager : MonoBehaviour
{
    public List<ColorButton> colorButtons;

    public AkibukiManager akibukiManager;

    public Action<int> OnUpdateButtonAction;



    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        foreach (var btn in colorButtons)
        {
            OnUpdateButtonAction += btn.UpdateButton;
        }
    }



    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        foreach (var btn in colorButtons)
        {
            OnUpdateButtonAction -= btn.UpdateButton;
        }
    }



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        colorButtons[0].OnClickColor();
    }


    public void SetPenColor(int id, Color color)
    {
        akibukiManager.SetPenColor(color);
        OnUpdateButtonAction?.Invoke(id);
    }




}
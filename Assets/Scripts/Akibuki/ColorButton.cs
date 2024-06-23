using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public int id;
    public ColorLayoutManager colorLayoutManager;
    public Image image;
    public Color color;


    public void OnClickColor()
    {
        colorLayoutManager.SetPenColor(id, color);
    }

    internal void UpdateButton(int ID)
    {
        if (image != null)
            image.enabled = id == ID;
    }


}

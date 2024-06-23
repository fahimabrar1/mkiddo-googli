using System;
using UnityEngine;

public class AkibukiManager : MonoBehaviour
{
    public LineGenerator lineGenerator;


    public void SetPenColor(Color color)
    {
        lineGenerator.SetPenColor(color);
    }

    internal void SetLineWidth(float val)
    {
        lineGenerator.SetLineWidth(val);
    }
}
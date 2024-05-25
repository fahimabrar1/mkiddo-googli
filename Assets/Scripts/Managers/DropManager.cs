using System;
using UnityEngine;

public class DropManager : LevelBaseManager
{// Enumeration for the two possible drop sides
    public enum DropSide
    {
        noSide,
        center,
        left,
        right,
    }

    public Timer timer;
    public virtual void OnDropObject(DraggableObject draggableObject, bool matched)
    { }


    public virtual void OnCancelDropObject(DraggableObject draggableObject)
    {
    }

    public virtual void PlayFailedAudio()
    {
    }
}
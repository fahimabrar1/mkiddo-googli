using System;
using UnityEngine;

public class DropManager : LevelBaseManager
{

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
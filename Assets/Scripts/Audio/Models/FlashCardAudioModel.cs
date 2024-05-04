
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlashCardAudioModel
{

    public List<AudioClip> clips;

    public int audioIndex = 0;

}

[Serializable]
public class DragAndDropAudioModel
{

    public List<AudioClip> clips;
    public int audioIndex = 0;

    public AudioClip failed;
    public AudioClip success;

}
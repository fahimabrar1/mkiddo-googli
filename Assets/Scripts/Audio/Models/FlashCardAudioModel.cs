
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

    public DragAndDropAudioModel()
    {
        sequqnceClips = new();
        initStart = new();
        failed = new();
        success = new();
    }

    public List<AudioClip> sequqnceClips;
    public int audioIndex = 0;

    public AudioClip initLetterAudio;
    public AudioClip initEnd;
    public AudioClip combinedAudio;
    public List<AudioClip> initStart;

    public List<AudioClip> failed;
    public List<AudioClip> success;

    internal void SetSequence()
    {
        sequqnceClips.Add(initStart[UnityEngine.Random.Range(0, initStart.Count)]);
        sequqnceClips.Add(initLetterAudio);
        sequqnceClips.Add(initEnd);
        sequqnceClips.Add(combinedAudio);
    }
}


[Serializable]
public class ImageSortingpAudioModel
{
    public AudioClip startingAudio;
    public AudioClip failedAudio;
    public AudioClip successAudio;

}
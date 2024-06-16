using System;
using System.Collections.Generic;
using UnityEngine;

public class HomePageAudioPlayer : MonoBehaviour
{

    public List<AudioClip> clips;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        var rand = UnityEngine.Random.Range(0, clips.Count);
        audioSource.clip = clips[rand];
        audioSource.Play();
    }
}

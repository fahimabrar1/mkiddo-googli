using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSortingAudioPLayer : MonoBehaviour
{
    public ImageSortingpAudioModel imageSortingAudioPLayers;
    private AudioSource audioSource;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }



    // Function to play a single audio clip
    public void PlayFirstAudioClip(AudioClip clip)
    {

        audioSource.clip = clip;
        audioSource.Play();
    }

    internal void PlayFailed(int currentFlashCard)
    {
        audioSource.clip = imageSortingAudioPLayers.failedAudio;
        audioSource.Play();
    }

    internal void PlaySuccess(int currentFlashCard)
    {
        audioSource.clip = imageSortingAudioPLayers.successAudio;
        audioSource.Play();
    }

}

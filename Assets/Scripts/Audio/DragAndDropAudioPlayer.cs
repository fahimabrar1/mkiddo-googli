using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropAudioPlayer : MonoBehaviour
{
    public DragAndDropAudioModel dragAndDropAudioPlayer;
    private AudioSource audioSource;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        dragAndDropAudioPlayer = new();
    }



    // Function to play a single audio clip
    public void PlayFirstAudioClip(int index)
    {

        audioSource.clip = dragAndDropAudioPlayer.sequqnceClips[dragAndDropAudioPlayer.audioIndex];
        audioSource.Play();
        dragAndDropAudioPlayer.audioIndex++;

    }

    // Function to play audio clips sequentially starting from the current clip index
    public void PlayAudioClipsSequentially(int currentFlashCard)
    {
        IEnumerator PlayAudioClipsSequentiallyCoroutine()
        {
            int totalAudioCount = dragAndDropAudioPlayer.sequqnceClips.Count;


            // Loop until the current index reaches the last audio clip
            while (dragAndDropAudioPlayer.audioIndex < totalAudioCount)
            {
                // Check if the audio source is not playing
                if (!audioSource.isPlaying)
                {
                    // Set the current audio clip and play it
                    audioSource.clip = dragAndDropAudioPlayer.sequqnceClips[dragAndDropAudioPlayer.audioIndex];
                    // Increment the index for the next audio clip
                    dragAndDropAudioPlayer.audioIndex++;
                    audioSource.Play();


                }
                // Yielding null here allows the loop to continue without waiting
                yield return null;
            }

            Debug.Log("All audio clips played.");
        }

        StartCoroutine(PlayAudioClipsSequentiallyCoroutine());
    }

    // internal void ResetAudioIndexForcard(int currentFlashCard)
    // {
    //     dragAndDropAudioPlayer[currentFlashCard].audioIndex = 0;
    // }

    internal void PlayFailed()
    {

        // Select a random audio clip from the array
        AudioClip randomFailedClip = dragAndDropAudioPlayer.failed[UnityEngine.Random.Range(0, dragAndDropAudioPlayer.failed.Count)];

        // Assign the random clip to the audio source and play it
        audioSource.clip = randomFailedClip;
        audioSource.Play();
    }

    internal void PlaySuccess()
    {
        // Select a random audio clip from the array
        AudioClip randomFailedClip = dragAndDropAudioPlayer.success[UnityEngine.Random.Range(0, dragAndDropAudioPlayer.success.Count)];

        // Assign the random clip to the audio source and play it
        audioSource.clip = randomFailedClip;
        audioSource.Play();
    }

    internal void OnsetCombinedAudio(AudioClip clip)
    {
        dragAndDropAudioPlayer.combinedAudio = clip;
    }


    internal void OnsetInitAudio(AudioClip clip)
    {
        dragAndDropAudioPlayer.initStart.Add(clip);
    }
    internal void OnsetFailedAudio(AudioClip clip)
    {
        dragAndDropAudioPlayer.failed.Add(clip);
    }

    internal void OnsetSuccessAudio(AudioClip clip)
    {
        dragAndDropAudioPlayer.success.Add(clip);
    }
    internal void OnsetInitEndAudio(AudioClip clip)
    {
        dragAndDropAudioPlayer.initEnd = clip;
    }
    internal void OnsetInitLetterAudio(AudioClip clip)
    {
        dragAndDropAudioPlayer.initLetterAudio = clip;
    }


    public void SetSequence()
    {
        dragAndDropAudioPlayer.SetSequence();
    }
}

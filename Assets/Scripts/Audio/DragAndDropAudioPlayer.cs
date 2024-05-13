using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropAudioPlayer : MonoBehaviour
{
    public List<DragAndDropAudioModel> dragAndDropAudioPlayer;
    private AudioSource audioSource;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }



    // Function to play a single audio clip
    public void PlayFirstAudioClip(int index)
    {
        if (index >= 0 && index < dragAndDropAudioPlayer.Count)
        {
            audioSource.clip = dragAndDropAudioPlayer[index].clips[dragAndDropAudioPlayer[index].audioIndex];
            audioSource.Play();
            dragAndDropAudioPlayer[index].audioIndex++;
        }
        else
        {
            Debug.LogWarning("Invalid audio clip index.");
        }
    }

    // Function to play audio clips sequentially starting from the current clip index
    public void PlayAudioClipsSequentially(int currentFlashCard)
    {
        IEnumerator PlayAudioClipsSequentiallyCoroutine()
        {
            int totalAudioCount = dragAndDropAudioPlayer[currentFlashCard].clips.Count;


            // Loop until the current index reaches the last audio clip
            while (dragAndDropAudioPlayer[currentFlashCard].audioIndex < totalAudioCount)
            {
                // Check if the audio source is not playing
                if (!audioSource.isPlaying)
                {
                    // Set the current audio clip and play it
                    audioSource.clip = dragAndDropAudioPlayer[currentFlashCard].clips[dragAndDropAudioPlayer[currentFlashCard].audioIndex];
                    // Increment the index for the next audio clip
                    dragAndDropAudioPlayer[currentFlashCard].audioIndex++;
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

    internal void PlayFailed(int currentFlashCard)
    {
        audioSource.clip = dragAndDropAudioPlayer[currentFlashCard].failed;
        audioSource.Play();
    }

    internal void PlaySuccess(int currentFlashCard)
    {
        audioSource.clip = dragAndDropAudioPlayer[currentFlashCard].success;
        audioSource.Play();
    }
}

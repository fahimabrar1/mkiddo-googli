using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashCardAudioPlayer : MonoBehaviour
{
    public List<FlashCardAudioModel> flashCardAudioModels;
    private AudioSource audioSource;
    private int currentClipIndex = 0;

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
        if (index >= 0 && index < flashCardAudioModels.Count)
        {
            audioSource.clip = flashCardAudioModels[index].clips[flashCardAudioModels[index].audioIndex];
            audioSource.Play();
            flashCardAudioModels[index].audioIndex++;
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
            int totalAudioCount = flashCardAudioModels[currentFlashCard].clips.Count;


            // Loop until the current index reaches the last audio clip
            while (flashCardAudioModels[currentFlashCard].audioIndex < totalAudioCount)
            {
                // Check if the audio source is not playing
                if (!audioSource.isPlaying)
                {
                    // Set the current audio clip and play it
                    audioSource.clip = flashCardAudioModels[currentFlashCard].clips[flashCardAudioModels[currentFlashCard].audioIndex];
                    audioSource.Play();
                    // Increment the index for the next audio clip
                    flashCardAudioModels[currentFlashCard].audioIndex++;
                }
                // Yielding null here allows the loop to continue without waiting
                yield return null;
            }

            Debug.Log("All audio clips played.");
        }

        StartCoroutine(PlayAudioClipsSequentiallyCoroutine());
    }

    internal void ResetAudioIndexForcard(int currentCardIndex)
    {
        flashCardAudioModels[currentCardIndex].audioIndex = 0;
    }
}

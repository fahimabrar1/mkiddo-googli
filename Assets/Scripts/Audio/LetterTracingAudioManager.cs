using System.Collections;
using System.Collections.Generic;
using IndieStudio.EnglishTracingBook.Game;
using UnityEngine;

public class LetterTracingAudioManager : MonoBehaviour
{
    public List<AudioClip> clips;
    public AudioClip Comepleteclip;

    public AudioSource audioSource;
    int rand = 0;
    // Start is called before the first frame update
    void Start()
    {
        rand = Random.Range(0, clips.Count);
    }



    public void PlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }


    // Call this method to start the sequence
    public void PlaySequence()
    {
        // Ensure there's at least one clip
        if (clips.Count > 0)
        {
            rand = Random.Range(0, clips.Count);
            StartCoroutine(PlayAudioSequence());
        }
        else
        {
            Debug.LogWarning("No audio clips available to play.");
        }
    }

    private IEnumerator PlayAudioSequence()
    {
        // Play a random clip
        audioSource.clip = clips[rand];
        audioSource.Play();

        // Wait until the clip has finished playing
        yield return new WaitWhile(() => audioSource.isPlaying);

        // Play the next clip from ShapesManager
        audioSource.clip = ShapesManager.GetCurrentShapesManager().GetCurrentShape().clip;
        audioSource.Play();
    }


    public void PlayOnSuccessClip()
    {
        PlayClip(Comepleteclip);
    }

}

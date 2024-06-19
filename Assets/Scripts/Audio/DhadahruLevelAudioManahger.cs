using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DhadahruLevelAudioManahger : MonoBehaviour
{

    public AudioSource audioSource;
    public List<AudioClip> praises;
    public void PlayRandomPraise()
    {

        audioSource.clip = praises[Random.Range(0, praises.Count)];
        audioSource.Play();
    }

}

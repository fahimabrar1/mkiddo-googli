using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class CanvasAudioManager : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip clip;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }




    public void OnCaptureImage()
    {
        audioSource.clip = clip;
        OnPlay();
    }



    public void OnPlay()
    {
        audioSource.Play();
    }
}

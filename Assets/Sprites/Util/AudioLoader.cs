using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AudioLoader : MonoBehaviour
{
    public AudioSource audioSource;

    public void LoadAudioClipCoroutine(string filePath, Action<AudioClip> OnLoadAudioClip)
    {
        StartCoroutine(LoadAudioCoroutine(filePath, OnLoadAudioClip));
    }

    private IEnumerator LoadAudioCoroutine(string filePath, Action<AudioClip> onLoadAudioClip)
    {
        string url = "file://" + filePath;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                onLoadAudioClip?.Invoke(clip);
                // Debug.Log($"Loaded audio: {Path.GetFileName(filePath)}");
            }
            else
            {
                onLoadAudioClip?.Invoke(null);
            }
        }
    }
}

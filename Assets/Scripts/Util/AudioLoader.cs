using System;
using System.Collections;
using System.IO;
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
        MyDebug.Log($"File Path: {filePath}");

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                byte[] audioData = www.downloadHandler.data;
                AudioClip clip = NAudioPlayer.FromMp3Data(audioData);

                onLoadAudioClip?.Invoke(clip);
                Debug.Log($"Loaded audio: {Path.GetFileName(filePath)}");
            }
            else
            {
                onLoadAudioClip?.Invoke(null);
            }
        }

        // using (var uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.UNKNOWN))
        // {
        //     ((DownloadHandlerAudioClip)uwr.downloadHandler).streamAudio = true;

        //     yield return uwr.SendWebRequest();

        //     if (uwr.result != UnityWebRequest.Result.Success)
        //     {
        //         MyDebug.LogError(uwr.error);
        //         yield break;
        //     }

        //     DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)uwr.downloadHandler;

        //     if (dlHandler.isDone)
        //     {
        //         AudioClip audioClip = dlHandler.audioClip;

        //         if (audioClip != null)
        //         {
        //             audioClip = DownloadHandlerAudioClip.GetContent(uwr);
        //             onLoadAudioClip?.Invoke(audioClip);

        //             Debug.Log("Playing song using Audio Source!");

        //         }
        //         else
        //         {
        //             Debug.Log("Couldn't find a valid AudioClip :(");
        //             onLoadAudioClip?.Invoke(null);

        //         }
        //     }
        //     else
        //     {
        //         Debug.Log("The download process is not completely finished.");
        //         onLoadAudioClip?.Invoke(null);
        //     }

        // }
        // else
        // {
        //     Debug.Log("Unable to locate converted song file.");
        // }
    }
}

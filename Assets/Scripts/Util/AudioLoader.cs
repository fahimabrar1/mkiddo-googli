using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AudioLoader : MonoBehaviour
{
    public AudioSource audioSource;


    public void LoadAudioClipFromWeb(string url, Action<AudioClip> onComplete)
    {
        StartCoroutine(DownloadAudioClipCoroutine(url, onComplete));
    }

    private IEnumerator DownloadAudioClipCoroutine(string url, Action<AudioClip> onComplete)
    {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN); // or AudioType.MPEG for MP3

        MyDebug.Log($"Downloading audio from URL: {url}");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            MyDebug.LogError($"Error downloading audio file: {www.error}");
            onComplete?.Invoke(null);
        }
        else
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            MyDebug.Log("Audio download completed.");
            onComplete?.Invoke(clip);
        }
    }
    public void LoadAudioClipCoroutine(string filePath, Action<AudioClip> OnLoadAudioClip)
    {
        StartCoroutine(LoadAudioCoroutine(filePath, OnLoadAudioClip));
    }

    private IEnumerator LoadAudioCoroutine(string filePath, Action<AudioClip> onLoadAudioClip)
    {
        string url = "";
#if UNITY_WEBGL && !UNITY_EDITOR
        url =  filePath;
        Debug.Log($"File Path: {filePath}");
#else
        url = "file://" + filePath;
        Debug.Log($"File Path: {filePath}");
#endif

        // using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        // {
        //     yield return www.SendWebRequest();

        //     if (www.result == UnityWebRequest.Result.Success)
        //     {
        //         byte[] audioData = www.downloadHandler.data;
        //         AudioClip clip = NAudioPlayer.FromMp3Data(audioData);

        //         onLoadAudioClip?.Invoke(clip);
        //         Debug.Log($"Loaded audio: {Path.GetFileName(url)}");
        //     }
        //     else
        //     {
        //         onLoadAudioClip?.Invoke(null);
        //     }
        // }

        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            ((DownloadHandlerAudioClip)uwr.downloadHandler).streamAudio = true;

            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                MyDebug.LogError(uwr.error);
                yield break;
            }

            DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)uwr.downloadHandler;

            if (dlHandler.isDone)
            {
                AudioClip audioClip = dlHandler.audioClip;

                if (audioClip != null)
                {
                    audioClip = DownloadHandlerAudioClip.GetContent(uwr);
                    onLoadAudioClip?.Invoke(audioClip);

                    Debug.Log("Playing song using Audio Source!");

                }
                else
                {
                    Debug.Log("Couldn't find a valid AudioClip :(");
                    onLoadAudioClip?.Invoke(null);

                }
            }
            else
            {
                Debug.Log("The download process is not completely finished.");
                onLoadAudioClip?.Invoke(null);
            }

        }



    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyWebRequest
{

    protected string baseUrl = "https://api.mkiddo.com";


    public IEnumerator DownloadAndUnzip(string url, string access_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJsb2dpbl9ieSI6Ik1TSVNETiIsImdvb2dsZV9pZCI6Ijk4NzQ1NjM3NDI4OTEtMzAiLCJ1aWQiOjM4MTg4LCJtc2lzZG4iOiI4ODAxNjg3MDU2MTQwIiwiZW1haWwiOiIiLCJzb3VyY2UiOiJhcHAiLCJhcHBfbmFtZSI6Im1LaWRkb192OjIuNi4xLmJldGEiLCJpYXQiOjE3MTI0MzA1MjcsImV4cCI6MTcxMjg2MjUyN30.oFouaGLiza11cSFODgS5TjqRWLgAjvntNM0A9HAwH0c")
    {
        UnityWebRequest www = UnityWebRequest.Get(baseUrl + url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            MyDebug.Log(www.error);
        }
        else
        {
            if (!Directory.Exists(Application.dataPath + "/googli"))
            {
                Directory.CreateDirectory(Application.dataPath + "/googli");
            }

            File.WriteAllBytes(Application.dataPath + "/googli/zips/test.zip", www.downloadHandler.data);
            ZipFile.ExtractToDirectory(Application.dataPath + "/googli/zips/test.zip", Application.persistentDataPath + "/googli/image_sort/test.zip");
            www.Dispose();
        }
    }



    // Method to fetch data from the specified URL
    public IEnumerator FetchData(string url, string blockSlug = "", string access_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJsb2dpbl9ieSI6Ik1TSVNETiIsImdvb2dsZV9pZCI6Ijk4NzQ1NjM3NDI4OTEtMzAiLCJ1aWQiOjM4MTg4LCJtc2lzZG4iOiI4ODAxNjg3MDU2MTQwIiwiZW1haWwiOiIiLCJzb3VyY2UiOiJhcHAiLCJhcHBfbmFtZSI6Im1LaWRkb192OjIuNi4xLmJldGEiLCJpYXQiOjE3MTI0MzA1MjcsImV4cCI6MTcxMjg2MjUyN30.oFouaGLiza11cSFODgS5TjqRWLgAjvntNM0A9HAwH0c", Action<OnApiResponseSuccess> OnApiResponseSucces = null, Action<OnApiResponseFailed> OnApiResponseFailed = null)
    {
        using UnityWebRequest www = UnityWebRequest.Get(baseUrl + url);
        // Add access token to request header if provided
        if (!string.IsNullOrEmpty(access_token))
        {
            www.SetRequestHeader("Authorization", "Bearer " + access_token);
        }
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch data: " + www.error);

            OnApiResponseFailed?.Invoke(new(www.error, www.responseCode));
        }
        else
        {
            // Deserialize the JSON response
            ApiResponse response = JsonUtility.FromJson<ApiResponse>(www.downloadHandler.text);
            VideoBlock _videoBlock = new();
            // Access the fetched data
            if (response != null && response.data != null && response.data.Video != null)
            {
                foreach (var videoBlock in response.data.Video)
                {
                    MyDebug.Log("Block Name: " + videoBlock.block_name);
                    if (videoBlock.block_slug.Equals(blockSlug))
                    {
                        _videoBlock = videoBlock;
                        break;
                    }
                }


                OnApiResponseSucces?.Invoke(new(_videoBlock, "Success", www.responseCode));
            }
            else
            {
                MyDebug.LogWarning("No data found in the response.");
            }
        }
    }




    // Method to fetch the image asynchronously from the specified URL
    public async void FetchImageAsync(string url, Image image)
    {
        using UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        var requestOperation = www.SendWebRequest();

        while (!requestOperation.isDone)
        {
            await Task.Yield(); // Yield control back to Unity until the request is done
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            MyDebug.LogError("Failed to fetch image: " + www.error);
        }
        else
        {
            // Get the downloaded texture
            Texture2D texture = DownloadHandlerTexture.GetContent(www);

            // Assign the texture to the image component
            if (image != null)
            {
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }
            else
            {
                MyDebug.LogWarning("Image component not assigned.");
            }
        }
    }

}

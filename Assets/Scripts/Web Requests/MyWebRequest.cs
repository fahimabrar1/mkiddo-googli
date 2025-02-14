using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyWebRequest
{

    protected string baseUrl = "https://api.mkiddo.com";

    public MyWebRequest()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/googli"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/googli");
        }
    }

    public IEnumerator DownloadAndUnzip(string url, string fileName, string gameType, int downloadID, Action<float, int> OnUpdateDownloadProgress, string access_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJsb2dpbl9ieSI6Ik1TSVNETiIsImdvb2dsZV9pZCI6Ijk4NzQ1NjM3NDI4OTEtMzAiLCJ1aWQiOjM4MTg4LCJtc2lzZG4iOiI4ODAxNjg3MDU2MTQwIiwiZW1haWwiOiIiLCJzb3VyY2UiOiJhcHAiLCJhcHBfbmFtZSI6Im1LaWRkb192OjIuNi4xLmJldGEiLCJpYXQiOjE3MTI0MzA1MjcsImV4cCI6MTcxMjg2MjUyN30.oFouaGLiza11cSFODgS5TjqRWLgAjvntNM0A9HAwH0c")
    {
        UnityWebRequest www = UnityWebRequest.Get(url);


        www.SendWebRequest();

        while (!www.isDone)
        {
            OnUpdateDownloadProgress?.Invoke(www.downloadProgress, downloadID);
            yield return new WaitForSeconds(0.05f);
        }

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.DataProcessingError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            MyDebug.Log($"Error for file:{fileName}, error: {www.result}");
        }
        else
        {
            if (!Directory.Exists(Application.temporaryCachePath + "/googli"))
            {
                MyDebug.LogWarning($"Creating {Application.temporaryCachePath}/googli path");
                Directory.CreateDirectory(Application.temporaryCachePath + "/googli");
            }

            if (!Directory.Exists(Application.temporaryCachePath + $"/googli/zips"))
            {
                MyDebug.LogWarning("Creating /googli/zips path");
                Directory.CreateDirectory(Application.temporaryCachePath + $"/googli/zips");
            }
            if (!Directory.Exists(Application.persistentDataPath + $"/googli/{gameType}"))
            {
                MyDebug.LogWarning($"Creating /googli/{gameType} path");
                Directory.CreateDirectory(Application.persistentDataPath + $"/googli/{gameType}");
            }

            MyDebug.Log("MWR Writing File to data path");
            File.WriteAllBytes(Application.temporaryCachePath + $"/googli/zips/{fileName}.zip", www.downloadHandler.data);
            MyDebug.Log("MWR Writing File to persistant path");
            ZipFile.ExtractToDirectory(Application.temporaryCachePath + $"/googli/zips/{fileName}.zip", Application.persistentDataPath + $"/googli/{gameType}/{fileName}", true);

            OnUpdateDownloadProgress?.Invoke(1, downloadID);

            www.Dispose();
        }
    }



    // Method to fetch data from the specified URL
    public IEnumerator FetchData(string url, int blockID = -1, string contentType = "", string access_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJsb2dpbl9ieSI6Ik1TSVNETiIsImdvb2dsZV9pZCI6Ijk4NzQ1NjM3NDI4OTEtMzAiLCJ1aWQiOjM4MTg4LCJtc2lzZG4iOiI4ODAxNjg3MDU2MTQwIiwiZW1haWwiOiIiLCJzb3VyY2UiOiJhcHAiLCJhcHBfbmFtZSI6Im1LaWRkb192OjIuNi4xLmJldGEiLCJpYXQiOjE3MTI0MzA1MjcsImV4cCI6MTcxMjg2MjUyN30.oFouaGLiza11cSFODgS5TjqRWLgAjvntNM0A9HAwH0c", Action<OnApiResponseSuccess> OnApiResponseSucces = null, Action<OnApiResponseFailed> OnApiResponseFailed = null)
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
                    if (videoBlock.block_id != -1 && videoBlock.block_id == blockID)
                    {
                        _videoBlock = videoBlock;

                    }

                    // if (videoBlock.contents.Count > 0)
                    // {
                    //     if (videoBlock.contents[0].content_type.Equals(contentType))
                    //     {
                    //         MyDebug.Log("Content Type Name: " + videoBlock.contents[0].content_type);
                    //         break;
                    //     }
                    // }

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

    // Method to fetch the image asynchronously from the specified URL
    public async void SendOTP(string url, string mobileNumber, Action<MyWebReqSuccessCallback> OnSuccessCallback, Action<MyWebReqFailedCallback> OnFailedCallback)
    {
        // Construct the JSON payload as a string
        string jsonPayload = $"{{\"msisdn\":\"{mobileNumber}\",\"source\":\"app\",\"app_name\":\"mKiddo_v:2.0.0\",\"app_signature\":\"xjnlFYUXJq3\"}}";

        // Create a UnityWebRequest for POST
        using UnityWebRequest www = new(baseUrl + url, UnityWebRequest.kHttpVerbPOST);

        // Convert the JSON string to a byte array
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        // Set the request body
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();

        // Set the content type to application/json
        www.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for the response
        var requestOperation = www.SendWebRequest();

        while (!requestOperation.isDone)
        {
            await Task.Yield(); // Yield control back to Unity until the request is done
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            OnFailedCallback?.Invoke(JsonUtility.FromJson<MyWebReqFailedCallback>(www.downloadHandler.text));
            Debug.LogError("Failed to send OTP: " + www.downloadHandler.text);
        }
        else
        {
            OnSuccessCallback?.Invoke(JsonUtility.FromJson<MyWebReqSuccessCallback>(www.downloadHandler.text));
            Debug.Log("OTP sent successfully: " + www.downloadHandler.text);
        }
    }

    public async void VerifyOTP(string url, string number, string otp, Action<MkiddOOnVerificationSuccessModel> OnSuccessCallback, Action<MyWebReqFailedCallback> OnFailedCallback)
    {
        number = number.Replace("+", "");
        // Construct the JSON payload as a string
        string jsonPayload = $"{{\"app_name\":\"mKiddo_v:2.6.1.beta\",\"source\":\"app\",\"login_by\":\"MSISDN\",\"msisdn\":\"{number}\",\"otp\":\"{otp}\"}}";

        MyDebug.Log(jsonPayload);

        // Create a UnityWebRequest for POST
        using UnityWebRequest www = new(baseUrl + url, UnityWebRequest.kHttpVerbPOST);

        // Convert the JSON string to a byte array
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        // Set the request body
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();

        // Set the content type to application/json
        www.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for the response
        var requestOperation = www.SendWebRequest();

        while (!requestOperation.isDone)
        {
            await Task.Yield(); // Yield control back to Unity until the request is done
        }

        if (www.result != UnityWebRequest.Result.Success)
        {

            OnFailedCallback?.Invoke(JsonUtility.FromJson<MyWebReqFailedCallback>(www.downloadHandler.text));
            Debug.LogError("Failed to verify OTP: " + www.downloadHandler.text);
        }
        else
        {
            OnSuccessCallback?.Invoke(JsonUtility.FromJson<MkiddOOnVerificationSuccessModel>(www.downloadHandler.text));
            Debug.Log("OTP verified successfully: " + www.downloadHandler.text);
        }
    }
}
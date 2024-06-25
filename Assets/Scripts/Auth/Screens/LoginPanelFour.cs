
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginPanelFour : LoginPanelBase
{

    public Image avatar;
    public GameObject button;
    public GameObject playBtn;
    public TMP_Text childName;

    public List<Sprite> avatars;

    void OnEnable()
    {
        button.SetActive(!loginScreenController.profileSO.isSignUsingGoogle);
        playBtn.SetActive(loginScreenController.profileSO.isSignUsingGoogle);
        // Check if signed in using Google
        if (loginScreenController.profileSO.isSignUsingGoogle)
        {


            // Load avatar from the URI
            if (loginScreenController.profileSO.ImageURI != null)
            {
                StartCoroutine(LoadImageFromURI(loginScreenController.profileSO.ImageURI));
            }
        }
        else
        {
            // Load avatar from the saved path in the app directory
            string savedAvatarPath = loginScreenController.profileSO.avatarPath;
            if (!string.IsNullOrEmpty(savedAvatarPath) && System.IO.File.Exists(savedAvatarPath))
            {
                StartCoroutine(LoadImageFromAppDir(savedAvatarPath));
            }
        }

        // Set the child's name
        string displayName = loginScreenController.profileSO.childName.Split(' ')[0];
        childName.text = "Let's Go " + displayName;
        childName.text = "Let's Go " + displayName;
    }

    public void LoggedIn()
    {
        PlayerPrefs.SetInt("is_logged_in", 1);
        PlayerPrefs.Save();
    }

    private IEnumerator LoadImageFromAppDir(string path)
    {
        string url = "file://" + path;

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                if (texture != null)
                {
                    // Create a sprite from the texture
                    Rect rect = new Rect(0, 0, texture.width, texture.height);
                    Vector2 pivot = new Vector2(0.5f, 0.5f);
                    Sprite sprite = Sprite.Create(texture, rect, pivot);

                    // Display the sprite in a UI Image and set to fill the avatar rect
                    avatar.sprite = sprite;
                    avatar.type = Image.Type.Simple;
                    avatar.preserveAspect = false;
                    avatar.SetNativeSize();
                }
                else
                {
                    Debug.LogError("Failed to load texture from " + path);
                }
            }
            else
            {
                Debug.LogError("UnityWebRequest error: " + uwr.error);
            }
        }
    }

    private IEnumerator LoadImageFromURI(Uri uri)
    {
        using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(uri);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            if (texture != null)
            {
                // Create a sprite from the texture
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                Sprite sprite = Sprite.Create(texture, rect, pivot);

                // Display the sprite in a UI Image and set to fill the avatar rect
                avatar.sprite = sprite;
                avatar.type = Image.Type.Simple;
                avatar.preserveAspect = false;
                avatar.SetNativeSize();
            }
            else
            {
                Debug.LogError("Failed to load texture from " + uri);
            }
        }
        else
        {
            Debug.LogError("UnityWebRequest error: " + uwr.error);
        }
    }
}
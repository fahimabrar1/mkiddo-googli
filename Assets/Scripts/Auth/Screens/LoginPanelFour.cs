
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelFour : LoginPanelBase
{

    public Image avatar;
    public TMP_Text childName;

    public List<Sprite> avatars;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        avatar.sprite = avatars[int.Parse(loginScreenController.profileSO.avatarIndex)];
        childName.text = loginScreenController.profileSO.childName;
    }

    public void LoggedIn()
    {
        PlayerPrefs.SetInt("logged_in", 1);
        PlayerPrefs.Save();
    }
}
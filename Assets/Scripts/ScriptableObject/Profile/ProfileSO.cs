using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Profile SO", menuName = "Game Data/ProfileSO", order = 0)]
public class ProfileSO : ScriptableObject
{
    // This is the UID of the user
    public int id = -1;
    public int child_id = -1;
    public string childName = "";
    public string month = "";
    public string day = "";
    public string year;
    public string avatarPath = "";
    public string countryCode = "+880";
    public string mobileNumber = "";

    public bool isSignUsingGoogle = false;
    public Uri ImageURI;


}
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Profile SO", menuName = "Game Data/ProfileSO", order = 0)]
public class ProfileSO : ScriptableObject
{
    public string childName;
    public string month;
    public string day;
    public string year;
    public string avatarPath = "0";
    public string countryCode = "+880";
    public string mobileNumber = "1234567890";

    public bool isSignUsingGoogle = false;
    public Uri ImageURI;


}
using UnityEngine;

[CreateAssetMenu(fileName = "Profile SO", menuName = "Game Data/ProfileSO", order = 0)]
public class ProfileSO : ScriptableObject
{
    public string childName;
    public string month;
    public string day;
    public string year;
    public string avatarIndex;
    public string countryCode;
    public string mobileNumber;


}
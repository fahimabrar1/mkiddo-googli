using UnityEngine;

public class MyPlayerPrefabs : MonoBehaviour
{
    public static MyPlayerPrefabs Instance;
    private ES3Settings es3settings;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        // Create a new ES3Settings to enable encryption.
        es3settings = new ES3Settings(ES3.EncryptionType.AES, "alpha-games");
    }


    public void SetString(string key, string value)
    {
        // Use the ES3Settings object to encrypt data.
        ES3.Save(key, value, es3settings);
    }

    public string GetString(string key, string defaultVal = "")
    {

        if (ES3.KeyExists(key))
            return ES3.Load<string>(key, es3settings);
        else
            return defaultVal;
    }


    public void SetInt(string key, int value)
    {
        // Use the ES3Settings object to encrypt data.
        ES3.Save(key, value, es3settings);
    }

    public int GetInt(string key, int defaultVal = -1)
    {

        if (ES3.KeyExists(key))
            return ES3.Load<int>(key, es3settings);
        else
            return defaultVal;
    }

}
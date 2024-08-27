using UnityEngine;

public class MyDebug
{

    public static void Log(string str)
    {
#if DEBUG || DEVELOPMENT
        Debug.Log(str);
#endif
    }
    public static void LogWarning(string str)
    {
#if DEBUG || DEVELOPMENT
        Debug.LogWarning(str);
#endif
    }
    public static void LogError(string str)
    {
#if DEBUG || DEVELOPMENT
        Debug.LogError(str);
#endif
    }

}
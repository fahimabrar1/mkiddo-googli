using System;

[Serializable]
public class MyWebReqFailedCallback
{
    public bool success;
    public int status_code;
    public string message;
}


[Serializable]
public class MyWebReqSuccessCallback
{
    public bool success;
    public int status_code;
    public string message;
    public string access_token;

}
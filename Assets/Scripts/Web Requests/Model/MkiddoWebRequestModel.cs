using System;
using System.Collections.Generic;

[Serializable]
public class Content
{
    public int show_id;
    public int content_id;
    public string content_type;
    public string type_icon;
    public string dedicated_for;
    public string name;
    public string link;
    public int sort;
    public string description;
    public string thumbnail;
    public string banner;
    public int duration;
    public string payment_type;
    public string general_type;
    public int sub_block_id;
    public int is_watched;
    public int watched_duration;
}

[Serializable]
public class VideoBlock
{
    public int block_id;
    public string block_name;
    public int sort;
    public string block_slug;
    public string block_orientation;
    public List<Content> contents;
}

[Serializable]
public class Data
{
    public List<VideoBlock> Video;
    // public List<object> Interactive; // If Interactive data is needed, you can create a model for it as well
}

[Serializable]
public class ApiResponse
{
    public int status_code;
    public bool success;
    public Data data;
}



[Serializable]
public class OnApiResponseSuccess
{
    public VideoBlock videoBlock;
    public string v;
    public long responseCode;

    public OnApiResponseSuccess(VideoBlock videoBlock, string v, long responseCode)
    {
        this.videoBlock = videoBlock;
        this.v = v;
        this.responseCode = responseCode;
    }
}


[Serializable]
public class OnDhadharuApiResponseSuccess
{
    public DhadharuData dhadharuData;
    public string v;
    public long responseCode;

    public OnDhadharuApiResponseSuccess(DhadharuData dhadharuData, string v, long responseCode)
    {
        this.dhadharuData = dhadharuData;
        this.v = v;
        this.responseCode = responseCode;
    }
}


[Serializable]
public class OnApiResponseFailed
{

    public OnApiResponseFailed(string message, long code)
    {
        this.message = message;
        this.code = code;
    }
    public string message;
    public long code;
}

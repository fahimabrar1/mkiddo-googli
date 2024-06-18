using System.Collections.Generic;

[System.Serializable]
public class ApiDhadharuResponse
{
    public bool success;
    public int status_code;
    public DhadharuData data;
}

[System.Serializable]
public class DhadharuData
{
    public List<QuizQuestion> math_quiz;
    public List<QuizQuestion> trivia_quiz;
    public List<QuizQuestion> puzzle;
}

[System.Serializable]
public class QuizQuestion
{
    public int question_id;
    public string game_type;
    public string question_text;
    public string question_image;
    public string question_audio;
    public string question_video;
    public string question_option;
    public string question_ans;
    public int question_point;
}
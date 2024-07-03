using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DhadharuContentFetcher : MonoBehaviour
{
    MyWebRequest myWebRequest;

    public List<GameObject> panels;

    public DhadharuDataSo dhadharuDataSo;

    public GameManager gameManager;

    [SerializeField]
    private DhadharuData dhadharuData;


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        myWebRequest = new();
        gameManager = FindAnyObjectByType<GameManager>();
        StartCoroutine(myWebRequest.FetchDhadharuData($"/api/v3/content/questions", PlayerPrefs.GetString("access_token"), OnApiResponseSucces: OnSuccessLoadingScreen));
    }
    private void OnSuccessLoadingScreen(OnDhadharuApiResponseSuccess onApiResponseSuccess)
    {


        dhadharuData = onApiResponseSuccess.dhadharuData;
        Initialize();
    }


    public void Initialize()
    {
        panels.ForEach((p) => p.SetActive(true));
    }



    public void OnClickWorldRiddle()
    {
        dhadharuDataSo.gameName = dhadharuData.puzzle[0].game_type;
        dhadharuDataSo.questions = dhadharuData.puzzle;
        PlayerPrefs.SetInt($"{dhadharuDataSo.gameName}_temp", -1);
        PlayerPrefs.Save();


        gameManager.LoadSceneAsync(2);

    }
    public void OnClickTriviaQuiz()
    {

        dhadharuDataSo.gameName = dhadharuData.trivia_quiz[0].game_type;
        //Todo: only keep english quizes
        dhadharuDataSo.questions = dhadharuData.trivia_quiz.Skip(20).ToList();
        PlayerPrefs.SetInt($"{dhadharuDataSo.gameName}_temp", -1);
        PlayerPrefs.Save();

        gameManager.LoadSceneAsync(3);


    }
    public void OnClickMathQuiz()
    {
        dhadharuDataSo.gameName = dhadharuData.math_quiz[0].game_type;
        dhadharuDataSo.questions = dhadharuData.math_quiz;
        PlayerPrefs.SetInt($"{dhadharuDataSo.gameName}_temp", -1);
        PlayerPrefs.Save();
        gameManager.LoadSceneAsync(4);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject OverlayObject;
    public GameObject GameOverObject;
    public GameObject ConfeittiObject;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }



    public void OnClickRestart()
    {
        gameManager.RestartLevel();
    }


    public void OnClickExit()
    {
        gameManager.LoadSceneAsync(1);
    }


    public void OnClickNext()
    {
        var levelBaseManager = FindObjectOfType<LevelBaseManager>();
        levelBaseManager.SaveLevel();
    }


    public void OnShowGameOverPanel()
    {
        ConfeittiObject.SetActive(true);
        OverlayObject.SetActive(true);
        GameOverObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }



    public void OnClickRestart()
    {
        gameManager.RestartLevel();
    }


    public void OnClickExit()
    {
        gameManager.LoadScene(0);
    }
}

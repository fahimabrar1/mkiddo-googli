using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class GameWinPanel : MonoBehaviour
{

    public Sprite FileldStart;
    public Sprite EmptyStar;
    public List<Image> Stars;

    public LevelBaseManager levelBaseManager;


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        levelBaseManager = FindObjectOfType<LevelBaseManager>();
        for (int i = 0; i < 3; i++)
        {
            Stars[i].sprite = levelBaseManager.StarCounts == 0 ? EmptyStar : levelBaseManager.StarCounts - 1 > i ? FileldStart : EmptyStar;
        }
    }

}

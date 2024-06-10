using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PanelData SO", menuName = "Game Data/PanelData SO", order = 0)]
public class PanelDataSO : ScriptableObject
{

    [SerializeField]
    public GamePanelData gamePanelData;

    [SerializeField]
    public string contentTypeFolderName;
    public string gameName;

}


[Serializable]
public class GamePanelData
{

    /// <summary>
    /// blockID for image sort: SORT_BY_RULE
    /// blockID for image sort: SORT_BY_RULE
    /// blockID for image sort: SORT_BY_RULE
    /// blockID for image sort: SORT_BY_RULE
    /// blockID for image sort: DRAG_N_DROP
    /// blockID for image sort: TWOSIDEMATCHING
    /// </summary>
    [SerializeField]
    public int blockID;

    /// <summary>
    /// below is the folder names, where we will keep the assets
    /// imgsort
    /// dnd
    /// imgmatch
    /// </summary>
    [SerializeField]
    public string gameTypeName;

    /// <summary>
    /// content_category:1 is for Sort-by-rule
    /// content_category:6 is for drag and drop
    /// content_category:1 is for mathcing 2 side
    /// </summary>
    [SerializeField]
    public string contentCategory;


    /// <summary>
    /// contentType for image sort: SORT_BY_RULE
    /// contentType for image sort: DRAG_N_DROP
    /// contentType for image sort: TWOSIDEMATCHING
    /// </summary>
    [SerializeField]
    public string contentType;


}
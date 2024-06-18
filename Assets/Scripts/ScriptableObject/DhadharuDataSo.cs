
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DhadharuData SO", menuName = "Game Data/DhadharuData SO", order = 0)]
public class DhadharuDataSo : ScriptableObject
{
    public string gameName;

    [SerializeField]
    public List<QuizQuestion> questions;
}
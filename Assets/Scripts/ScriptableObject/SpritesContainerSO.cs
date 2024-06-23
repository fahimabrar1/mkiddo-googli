using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpritesContainer SO", menuName = "Game Data/Sprites Container SO", order = 0)]
public class SpritesContainerSO : ScriptableObject
{
    [SerializeField]
    public List<SpriteContainer> stickerContainers;
}



[Serializable]
public class SpriteContainer
{
    public Sprite sprite;
    public List<Sprite> stickers;
}
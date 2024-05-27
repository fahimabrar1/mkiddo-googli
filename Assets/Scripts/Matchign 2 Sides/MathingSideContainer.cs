using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Matching2SidesManager;

public class MathingSideContainer : MonoBehaviour
{


    public Matching2SidesManagerType matching2SidesManagerType;
    public List<Vector3> sidesLocations = new()
    {
        new (3.25f, -1, 0),
        new (3.25f, 1, 0),
        new (3.25f, 3, 0),
        new (3.25f, -3, 0),
    };


    public GameObject sidesPrefab; // List of sprite renderers in this container
    public List<MatchingSide> sides; // List of sprite renderers in this container

    // Method to set sprites
    // Method to set sprites with shuffling
    public void SetSprites(List<Sprite> sprites)
    {


        // Shuffle the sprites list
        List<int> indices = new List<int>();
        for (int i = 0; i < sprites.Count; i++)
        {
            indices.Add(i);
        }
        indices.Shuffle();

        // Assign the shuffled sprites to the sprite renderers and update IDs
        for (int i = 0; i < sprites.Count; i++)
        {
            int shuffledIndex = indices[i];
            var sideObj = Instantiate(sidesPrefab, (matching2SidesManagerType == Matching2SidesManagerType.left) ? new(sidesLocations[i].x * -1, sidesLocations[i].y, sidesLocations[i].z) : sidesLocations[i], Quaternion.identity, this.transform);
            if (sideObj.TryGetComponent(out MatchingSide side))
            {

                side.spriteRenderer.sprite = sprites[shuffledIndex];
                side.ID = shuffledIndex + 1; // Assign a new ID based on the shuffled index
                side.matching2SidesManagerType = matching2SidesManagerType; // Assign a side
            }
        }
    }
}

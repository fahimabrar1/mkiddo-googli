using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageMatchignCardHolder : MonoBehaviour
{

    public List<ImageMatchingCard> cards;
    public List<ImageMatchingCard> cardsFormatch;

    private int toBeMatch = 0;

    // Start is called before the first frame update
    void Start()
    {
        cardsFormatch = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.TryGetComponent(out ImageMatchingCard imageMatchingCard))
            {
                imageMatchingCard.CardID = i;
                imageMatchingCard.imageMatchignCardHolder = this;

                cards.Add(imageMatchingCard);
            }
        }
    }



    public void OnClickForMatch(ImageMatchingCard imageMatchingCard)
    {
        if (cardsFormatch.Count < 2)
            cardsFormatch.Add(imageMatchingCard);
        if (cardsFormatch.Count == 2)
        {
            if (cardsFormatch[0].CombinationID == cardsFormatch[1].CombinationID)
            {
                foreach (var card in cardsFormatch)
                {
                    card.FadeOutBottomToTop();
                }

                cardsFormatch.Clear();
                toBeMatch++;

                if (toBeMatch == 5)
                {
                    //Todo:GameOver
                }
            }
            else
            {
                // then it's  not a match
                foreach (var card in cardsFormatch)
                {
                    StartCoroutine(card.RotateCard180To90Deg());
                }
                cardsFormatch.Clear();
            }
        }
    }
}

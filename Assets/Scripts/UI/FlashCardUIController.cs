using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashCardUIController : MonoBehaviour
{
    public Button close;
    public Button previous;
    public Button next;

    public FlashCardManager flashCardManager;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if (flashCardManager.currentCardIndex == 0)
        {
            previous.gameObject.SetActive(false);
        }
    }



    public void GoToNextCard()
    {


        flashCardManager.GoToNextCard();

        Debug.Log($"Current Index:{flashCardManager.currentCardIndex}, Total count: {flashCardManager.flashCards.Count}");
        if (flashCardManager.currentCardIndex > 0)
        {
            previous.gameObject.SetActive(true);
        }

        if (flashCardManager.currentCardIndex + 1 == flashCardManager.flashCards.Count)
        {
            next.gameObject.SetActive(false);
        }

    }

    public void GoToPreviousCard()
    {

        flashCardManager.GoToPreviousCard();
        Debug.Log($"Current Index:{flashCardManager.currentCardIndex}, Total count: {flashCardManager.flashCards.Count}");

        if (flashCardManager.currentCardIndex == 0)
        {
            previous.gameObject.SetActive(false);
        }

        if (flashCardManager.currentCardIndex < flashCardManager.flashCards.Count - 1)
        {
            next.gameObject.SetActive(true);
        }
    }
}

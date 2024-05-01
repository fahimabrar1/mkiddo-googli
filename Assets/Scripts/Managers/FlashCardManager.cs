using System;
using System.Collections.Generic;
using UnityEngine;

public class FlashCardManager : MonoBehaviour
{

    public List<FlashCardHolder> flashCards = new();

    public FlashCardAudioPlayer flashCardAudioPlayer;

    public int currentCardIndex;




    public void OnFlashInitailizedComplete()
    {
        //Todo: Play Audio
        flashCardAudioPlayer.PlayFirstAudioClip(currentCardIndex);
    }

    public void OnFlashClosedComplete()
    {
    }


    public void OnFlashOpenedComplete()
    {
        //Todo: Play Audio

        flashCardAudioPlayer.PlayAudioClipsSequentially(currentCardIndex);
    }

    internal void GoToNextCard()
    {
        flashCardAudioPlayer.ResetAudioIndexForcard(currentCardIndex);
        flashCards[currentCardIndex].gameObject.SetActive(false);
        currentCardIndex++;
        flashCards[currentCardIndex].gameObject.SetActive(true);
    }

    internal void GoToPreviousCard()
    {
        flashCardAudioPlayer.ResetAudioIndexForcard(currentCardIndex);
        flashCards[currentCardIndex].gameObject.SetActive(false);
        currentCardIndex--;
        flashCards[currentCardIndex].gameObject.SetActive(true);
    }
}

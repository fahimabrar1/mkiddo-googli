using System;
using UnityEngine;
using UnityEngine.Events;

public class FlashCardHolder : MonoBehaviour
{
    public int ID = 0;
    public FlashCard flashCardBack;
    public FlashCard flashCardFront;


    public UnityEvent OnFlashInitailizedCompleteEvnet;
    public UnityEvent OnFlashClosedCompleteEvent;
    public UnityEvent OnFlashOpenedCompleteEvent;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        Debug.Log("Enabled");
        flashCardBack.transform.rotation = Quaternion.Euler(0, 90, 0);
        flashCardFront.transform.rotation = Quaternion.Euler(0, 90, 0);
        flashCardBack.FlashBackSide();
    }




    public void OnFlashInitailizedComplete()
    {
        //Todo: Play Audio
        OnFlashInitailizedCompleteEvnet?.Invoke();
    }

    public void OnFlashClosedComplete()
    {
        StartCoroutine(flashCardFront.RotateCard90To0Deg());
    }


    public void OnFlashOpenedComplete()
    {
        //Todo: Play Audio
        OnFlashOpenedCompleteEvent?.Invoke();

    }
}

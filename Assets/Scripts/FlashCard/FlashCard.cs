using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class FlashCard : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Sprite frontFace;
    public Sprite backFace;

    public Transform backTransform;
    public Transform FrontTransform;

    public bool isBack;
    bool coroutineAllowed;

    public float initializationDuration;
    public float rotationDuration;

    public UnityEvent OnFlashInitializaitonComplete;
    public UnityEvent OnFlashComplete0to90Deg;
    public UnityEvent OnFlashComplete90to0Deg;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    internal void Initialize()
    {
        spriteRenderer.sprite = isBack ? backFace : frontFace;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        coroutineAllowed = true;
    }



    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    public void OnMouseDown()
    {
        Debug.Log("Touch");
        if (coroutineAllowed && isBack)
            StartCoroutine(RotateCard0To90Deg());
    }

    private IEnumerator RotateCard0To90Deg()
    {
        coroutineAllowed = false;

        transform.DORotate(new Vector3(0f, 90f, 0f), rotationDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => OnFlashComplete0to90Deg?.Invoke());

        yield return new WaitForSeconds(rotationDuration);

        coroutineAllowed = true;
    }

    public IEnumerator RotateCard90To0Deg()
    {
        coroutineAllowed = false;

        transform.DORotate(new Vector3(0f, 0f, 0f), rotationDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => OnFlashComplete90to0Deg?.Invoke());

        yield return new WaitForSeconds(rotationDuration);

        coroutineAllowed = true;
    }

    private IEnumerator InitializeRotate90To0()
    {
        transform.DORotate(new Vector3(0f, 0f, 0f), initializationDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => OnFlashInitializaitonComplete?.Invoke());

        yield return new WaitForSeconds(rotationDuration);
    }

    internal void FlashBackSide()
    {
        StartCoroutine(InitializeRotate90To0());
        //Todo: now flash the card
    }

}
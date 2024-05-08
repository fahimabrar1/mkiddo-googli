using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

[Serializable]
public class UnityEventImageMatchingCard : UnityEvent<ImageMatchingCard> { }
public class ImageMatchingCard : MonoBehaviour
{
    public int CardID;
    public int CombinationID;

    public bool isBack;
    public bool isClicked;
    bool coroutineAllowed;

    public float initializationDuration;
    public float rotationDuration;
    public float randomZ;
    public SpriteRenderer spriteRenderer;
    public Sprite backSprite;
    public Sprite frontSprite;

    public ImageMatchignCardHolder imageMatchignCardHolder;

    public UnityEvent OnFlashInitializationComplete;
    public UnityEventImageMatchingCard OnFlashComplete90to180Deg;
    public UnityEvent OnFlashComplete180to90Deg;
    public UnityEvent OnFlashComplete90to0Deg;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        randomZ = isBack ? (UnityEngine.Random.Range(0, 2) == 0 ? -3.5f : 3.5f) : 0f;
        isClicked = false;
        isBack = true;
        coroutineAllowed = true;
        FlashBackSide();


    }


    public void OnMouseDown()
    {
        Debug.Log("Touch");
        if (coroutineAllowed && isBack && !isClicked)
        {
            StartCoroutine(RotateCard0To90Deg());
        }
    }

    // Rotate from 0 to 90 degrees
    private IEnumerator RotateCard0To90Deg()
    {
        coroutineAllowed = false;
        isBack = true;
        isClicked = true;
        spriteRenderer.sprite = backSprite;
        transform.DORotate(new Vector3(0f, 90f, isBack ? randomZ : 0f), rotationDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => StartCoroutine(RotateCard90To180Deg()));

        yield return new WaitForSeconds(rotationDuration);

        coroutineAllowed = true;
    }

    // Rotate from 90 to 180 degrees
    public IEnumerator RotateCard90To180Deg()
    {
        coroutineAllowed = false;
        isBack = false;
        spriteRenderer.sprite = frontSprite;
        transform.DORotate(new Vector3(0f, 180f, 0f), rotationDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => OnFlashComplete90to180Deg?.Invoke(this));

        yield return new WaitForSeconds(rotationDuration);

        coroutineAllowed = true;
    }

    // Rotate from 180 to 90 degrees
    public IEnumerator RotateCard180To90Deg()
    {
        coroutineAllowed = false;
        isBack = false;
        transform.DORotate(new Vector3(0f, 90f, 0f), rotationDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => StartCoroutine(RotateCard90To0Deg()));

        yield return new WaitForSeconds(rotationDuration);

        coroutineAllowed = true;
    }

    // Rotate from 90 to 0 degrees
    public IEnumerator RotateCard90To0Deg()
    {
        coroutineAllowed = false;
        isBack = true;
        spriteRenderer.sprite = backSprite;
        transform.DORotate(new Vector3(0f, 0f, isBack ? randomZ : 0f), rotationDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => OnFlashComplete90to0Deg?.Invoke());

        yield return new WaitForSeconds(rotationDuration);
        isClicked = false;
        coroutineAllowed = true;
    }

    // Initialize by rotating from 90 to 0 degrees
    private IEnumerator InitializeRotate90To0()
    {
        isBack = true;
        spriteRenderer.sprite = backSprite;
        transform.DORotate(new Vector3(0f, 0f, 0f), initializationDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => OnFlashInitializationComplete?.Invoke());

        yield return new WaitForSeconds(initializationDuration);
    }

    // Flash back side of the card
    internal void FlashBackSide()
    {
        StartCoroutine(InitializeRotate90To0());
        // Todo: now flash the card
    }

    internal void FadeOutBottomToTop()
    {
        // Disable any other animations while this one is playing
        coroutineAllowed = false;

        // Create a DOTween sequence
        Sequence sequence = DOTween.Sequence();

        // Bounce animation
        sequence.Append(transform.DOMoveY(transform.position.y + 1f, 1f)
            .SetEase(Ease.OutQuad));

        // Fade animation
        sequence.Join(spriteRenderer.DOFade(0f, 1f));

        // Callback to re-enable animations
        sequence.OnComplete(() =>
        {
            // Enable animations again after sequence completes
            gameObject.SetActive(false);
        });
    }
}

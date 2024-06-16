using UnityEngine;
using DG.Tweening;

public class FadeEffect : MonoBehaviour
{
    public float fadeTime = 0.3f;  // Time it takes to fade in
    private CanvasGroup canvasGroup;
    private Tween fadeTween;

    void Awake()
    {
        // Try to get the CanvasGroup component attached to this GameObject
        canvasGroup = GetComponent<CanvasGroup>();

        // If there is no CanvasGroup, add one
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void OnEnable()
    {
        // Reset alpha to 0 at the start
        canvasGroup.alpha = 0f;

        // Fade in the CanvasGroup and store the tween
        fadeTween = canvasGroup.DOFade(1f, fadeTime);
    }

    void OnDisable()
    {
        // Kill the fade tween if it exists
        if (fadeTween != null && fadeTween.IsActive())
        {
            fadeTween.Kill();
        }
    }
}

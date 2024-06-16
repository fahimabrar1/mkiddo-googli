using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // For IPointerDownHandler and IPointerUpHandler
using DG.Tweening; // DOTween namespace

public class ButtonSmallOnHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float shrinkDuration = 0.1f; // Duration of the shrink effect
    public float shrinkScale = 0.9f; // Scale to shrink to

    private Vector3 originalScale; // To store the original scale of the button
    private Button button;
    private Tween currentTween; // Reference to the current tween

    public void OnPointerDown(PointerEventData eventData)
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            originalScale = transform.localScale; // Store original scale
        }
        // Animate the button to shrink
        currentTween = transform.DOScale(shrinkScale, shrinkDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Animate back to original scale
        currentTween = transform.DOScale(originalScale, shrinkDuration).SetEase(Ease.OutQuad);
    }

    private void OnDisable()
    {
        // Check if the tween is active before killing it
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
    }
}

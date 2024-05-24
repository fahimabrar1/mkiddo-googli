using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MyTween
{
    // Private fields
    private Sequence _tweenUpDownSequence;
    private Sequence _tweenLeftRightSequence;
    private Sequence _tweenDiagonalSequence;

    private Transform _transform;

    // Constructor
    public MyTween(Transform transform)
    {
        _transform = transform;
    }

    // Stops a specific tween sequence
    public void StopTween(int tweenIndex)
    {
        switch (tweenIndex)
        {
            case 0:
                _tweenUpDownSequence?.Kill();
                break;
            case 1:
                _tweenLeftRightSequence?.Kill();
                break;
            case 2:
                _tweenDiagonalSequence?.Kill();
                break;
        }
    }

    // Moves the transform back to a target position
    public void TweenBackToPosition(Vector3 targetposition, float duration, Action OnCompleteCallback = null)
    {
        _transform.DOMove(targetposition, duration).SetAutoKill(true).OnComplete(() => OnCompleteCallback?.Invoke());
    }

    // Tweens the transform up and down
    public void TweenUpDown(float distance, float duration)
    {
        _tweenUpDownSequence = DOTween.Sequence();
        Vector3 up = new(0, distance, 0);
        Vector3 down = new(0, -distance, 0);

        _tweenUpDownSequence.Append(_transform.DOMove(_transform.position + up, duration).SetEase(Ease.InOutQuad))
                          .Append(_transform.DOMove(_transform.position + down, duration).SetEase(Ease.InOutQuad))
                          .SetLoops(-1, LoopType.Yoyo);
    }

    // Tweens the transform left and right
    public void TweenLeftRight(float distance, float duration)
    {
        _tweenLeftRightSequence = DOTween.Sequence();
        Vector3 right = new(distance, 0, 0);
        Vector3 left = new(-distance, 0, 0);

        _tweenLeftRightSequence.Append(_transform.DOMove(_transform.position + right, duration).SetEase(Ease.InOutQuad))
                  .Append(_transform.DOMove(_transform.position + left, duration).SetEase(Ease.InOutQuad))
                  .SetLoops(-1, LoopType.Yoyo);
    }

    // Tweens the transform diagonally
    public void TweenDiagonal(float distance, float duration)
    {
        _tweenDiagonalSequence = DOTween.Sequence();
        Vector3 diagonalUpRight = new(distance, distance, 0);
        Vector3 diagonalDownLeft = new(-distance, -distance, 0);

        _tweenDiagonalSequence.Append(_transform.DOMove(_transform.position + diagonalUpRight, duration).SetEase(Ease.InOutQuad))
                  .Append(_transform.DOMove(_transform.position + diagonalDownLeft, duration).SetEase(Ease.InOutQuad))
                  .SetLoops(-1, LoopType.Yoyo);
    }
}
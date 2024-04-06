using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MyTween
{
    private Sequence tweenUpDownSequence;
    private Sequence tweenLeftRightSequence;
    private Sequence tweenDiagonalSequence;


    public void StopTween(int tweenIndex)
    {
        switch (tweenIndex)
        {
            case 0:
                tweenUpDownSequence?.Kill();
                break;
            case 1:
                tweenLeftRightSequence?.Kill();
                break;
            case 2:
                tweenDiagonalSequence?.Kill();
                break;
        }
    }
    public void TweenUpDown(Transform transform, float distance, float duration)
    {
        tweenUpDownSequence = DOTween.Sequence();
        Vector3 up = new(0, distance, 0);
        Vector3 down = new(0, -distance, 0);

        tweenUpDownSequence.Append(transform.DOMove(transform.position + up, duration).SetEase(Ease.InOutQuad))
                   .Append(transform.DOMove(transform.position + down, duration).SetEase(Ease.InOutQuad))
                   .SetLoops(-1, LoopType.Yoyo);
    }
    public void TweenLeftRight(Transform transform, float distance, float duration)
    {
        tweenLeftRightSequence = DOTween.Sequence();
        Vector3 right = new(distance, 0, 0);
        Vector3 left = new(-distance, 0, 0);

        tweenLeftRightSequence.Append(transform.DOMove(transform.position + right, duration).SetEase(Ease.InOutQuad))
                   .Append(transform.DOMove(transform.position + left, duration).SetEase(Ease.InOutQuad))
                   .SetLoops(-1, LoopType.Yoyo);


    }


    public void TweenDiagonal(Transform transform, float distance, float duration)
    {
        tweenDiagonalSequence = DOTween.Sequence();
        Vector3 diagonalUpRight = new(distance, distance, 0);
        Vector3 diagonalDownLeft = new(-distance, -distance, 0);

        tweenDiagonalSequence.Append(transform.DOMove(transform.position + diagonalUpRight, duration).SetEase(Ease.InOutQuad))
                   .Append(transform.DOMove(transform.position + diagonalDownLeft, duration).SetEase(Ease.InOutQuad))
                   .SetLoops(-1, LoopType.Yoyo);
    }
}

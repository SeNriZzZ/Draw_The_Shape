
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RoutePart : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float maxAlpha;
    [SerializeField] private float intervalForHide;

    private Sequence sequence;
    public void Show()
    {
        image.SetActive();
        image.color = image.color.WithAlpha(0f);
        sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(maxAlpha, fadeDuration));
        sequence.AppendInterval(intervalForHide);
        sequence.Append(image.DOFade(0, fadeDuration));
        sequence.SetLoops(-1, LoopType.Yoyo);
    }

    public void Hide()
    {
        sequence.Kill();
        image.DOKill();
        image.SetInactive();
    }
}
